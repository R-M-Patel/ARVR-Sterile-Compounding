using UnityEngine;
using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class ObjectDetection : MonoBehaviour {
	public Leap.Unity.LeapServiceProvider leapProvider;
	Controller leapController;
	public List<Vector3> vectors = new List<Vector3>(); //list that holds all tracked 3D points
	public byte brightnessThreshold = 136; //threshold for detecting retroreflective sticker
	public bool drawDebug;
	int rightCount = 0;
	int leftCount = 0;
	float[] rightVectors = new float[765];
	float[] leftVectors = new float[765];
	int[] MatchVectors = new int[255];
	public Vector3 cameraOffset = new Vector3(0.02f, 0f, 0f);
	int pixelLimit = 1000;
	int pixelsFound = 0;
	public float sizeCutoff = 500000f;
	byte[] rightImageData;
	byte[] leftImageData;
	int Width = 640;
	int Height = 240;
	bool checkingImageState;
	bool imagesEnabled;
	protected long imageTimeout = 9000; //in microseconds

  	void Start() {
    	leapController = leapProvider.GetLeapController(); //set up the leap controller
    	SetDimensions(Width, Height); //set the byte arrays
  	}//end-Start

	//reset everything for this frame
	void Update() {
		vectors.Clear();
    	pixelsFound = 0;
    	for (int i = 0; i < 765; i++) {
      		rightVectors[i] = 0;
    	}//end-for
    	
    	for (int i = 0; i < 765; i++) {
      		leftVectors[i] = 0;
    	}//end-for
    	
    	for (int i = 0; i < 255; i++) {
      		MatchVectors[i] = 256;
    	}//end-for

    	Image _requestedImage = leapController.RequestImages(leapProvider.CurrentFrame.Id, Image.ImageType.DEFAULT); //get the leap device

    	if (_requestedImage.Width == 0 || _requestedImage.Height == 0) {
      		Debug.Log("No data in the images.");
      		return;
    	}//end-if

    	if(_requestedImage.Width != Width || _requestedImage.Height != Height){
      		Width = _requestedImage.Width;
      		Height = _requestedImage.Height;
      		SetDimensions(Width, Height); //change byte array size since image dimensions changed
    	}//end-if

    	//put images into the byte arrays
    	long start = leapController.Now();
    	while(!_requestedImage.IsComplete){
      		if(leapController.Now() - start > imageTimeout)
        	break;
    	}//end-while
    
    	byte[] packedImages = _requestedImage.Data;
    	if(_requestedImage.IsComplete){
      		System.Array.Copy(packedImages, 0, rightImageData, 0, rightImageData.Length);
      		System.Array.Copy(packedImages, _requestedImage.Width * _requestedImage.Height, leftImageData, 0, leftImageData.Length);
    	}//end-if

    	//find and add vectors to right array
    	rightCount = 0;
    	int rightBiggest = 0;
    	for(int i = (Width * Height) - 1; i > 0; i--){
      		if((pixelsFound < pixelLimit) && (rightImageData[i] > brightnessThreshold)){
        		Vector3 v = VectorFind(i, rightImageData, 0, 0);
        		if(v.z > 10){
          			rightVectors[rightCount * 3] = v.x;
          			rightVectors[(rightCount * 3) + 1] = v.y;
          			rightVectors[(rightCount * 3) + 2] = v.z;
          			if(v.z > rightVectors[rightBiggest + 2]){
            			rightBiggest = rightCount * 3;
          			}//end-if
          			rightCount++;
        		}//end-if
      		}//end-if
    	}//end-for

    	//find and add vectors to left array
		leftCount = 0;
    	int lBiggest = 0;
    	for(int i = (Width * Height) - 1; i > 0; i--){
			if((pixelsFound < pixelLimit) && (leftImageData[i] > brightnessThreshold)){
				Vector3 v = VectorFind(i, leftImageData, 0, 1);
				if(v.z > 10){
					leftVectors[leftCount * 3] = v.x;
					leftVectors[(leftCount * 3) + 1] = v.y;
					leftVectors[(leftCount * 3) + 2] = v.z;
					if(v.z > leftVectors[lBiggest + 2]){
						lBiggest = leftCount * 3;
					}//end-if
					leftCount++;
				}//end-if
			}//end-if
    	}//end-for

    	if(pixelLimit == pixelsFound){
      		Debug.Log("Data is unreliable. Too many bright pixels");
    	}//end-if
{//?
    
    
		//convert rays to leap space for right vectors
      	for(int i = 0; i < (rightCount * 3); i = (i + 3)){
        	Vector fVector = _requestedImage.PixelToRectilinear(Image.PerspectiveType.STEREO_LEFT, new Vector(((rightVectors[i] / rightVectors[i + 2])), ((rightVectors[i + 1] / rightVectors[i + 2])), 0));
        	rightVectors[i] = -fVector.x * rightVectors[i + 2]; //x scalar;
        	rightVectors[i + 1] = -fVector.y * rightVectors[i + 2]; //y scalar;
      	}//end-for

      	//convert rays to leap space for left vectors
      	for(int i = 0; i < (leftCount * 3); i = (i + 3)){
        	Vector fVector = _requestedImage.PixelToRectilinear(Image.PerspectiveType.STEREO_RIGHT, new Vector(((leftVectors[i] / leftVectors[i + 2])), ((leftVectors[i + 1] / leftVectors[i + 2])), 0));
        	leftVectors[i] = -fVector.x * leftVectors[i + 2];//x scalar;
        	leftVectors[i + 1] = -fVector.y * leftVectors[i + 2];//y scalar;
      	}//end-for

      	//match the left arrays to the right arrays
      	//make sure the rays are at least within 0.05 of each other
      	//pick a pair that results in longest distance to avoid entanglement
      	float farthest;
      	for(int i = 0; i < (rightCount * 3); i = (i + 3)){
        	if(rightVectors[i + 2] < sizeCutoff){
          		Vector3 RDir = new Vector3((rightVectors[i] / rightVectors[i + 2]), 1f, rightVectors[i + 1] / rightVectors[i + 2]);
          
          		if(drawDebug){
            		Debug.DrawRay(transform.TransformPoint(cameraOffset), transform.TransformPoint(new Vector3(10f * RDir.x, 10f * RDir.y, 10f * RDir.z)), Color.red);
          		}//end-if
          
          		farthest = 0.02f; //needs to be farther than this
          		for(int j = 0; j < (leftCount * 3); j = (j + 3)){
					if(leftVectors[j + 2] < sizeCutoff){
						Vector3 LDir = new Vector3(leftVectors[j] / leftVectors[j + 2], 1f, leftVectors[j + 1] / leftVectors[j + 2]);
			  
						if(drawDebug){
							Debug.DrawRay(transform.TransformPoint(-cameraOffset), transform.TransformPoint(new Vector3(10f * LDir.x, 10f * LDir.y, 10f * LDir.z)), Color.cyan);
						}//end-if
			  
						float intersectiondistance = ClosestDistOfApproach(cameraOffset, RDir, -cameraOffset, LDir); //test all sfor farthest working vector
						float distancefromleap = ClosestTimeOfApproach(cameraOffset, RDir, -cameraOffset, LDir);
			  
						if(drawDebug){
							Debug.DrawLine(transform.TransformPoint(ClosestPointOfApproach(cameraOffset, RDir, -cameraOffset, LDir)), transform.TransformPoint(ClosestPointOfApproach(-cameraOffset, LDir, CameraOffset, RDir)));
						}//end-if
			  
						if(distancefromleap > farthest){
							if(intersectiondistance < 0.015f){
								farthest = distancefromleap;
								MatchVectors[i / 3] = j;
							}//end-if
						}//end-if
					}//end-if
          		}//end-for
			}//end-if
		}//end-for


		//add vector positions relative to parent only at the intersection of matched rays
      	for(int i = 0; i < rightCount; i++){
			if((MatchVectors[i] != 256)){
          		Vector3 RDir = new Vector3((rightVectors[(i * 3)] / rightVectors[(i * 3) + 2]), 1f, rightVectors[(i * 3) + 1] / rightVectors[(i * 3) + 2]);
          		Vector3 LDir = new Vector3(leftVectors[MatchVectors[i]] / leftVectors[MatchVectors[i] + 2], 1f, leftVectors[MatchVectors[i] + 1] / leftVectors[MatchVectors[i] + 2]);
          		Vectors.Add(transform.TransformPoint(ClosestPointOfApproach(cameraOffset, RDir, -cameraOffset, LDir)));
        	}//end-if
      	}//end-for
    }//end-Update
}//?



	//draw spheres at intersections of matched rays
	void OnDrawGizmos(){
    	if(drawDebug){
      		for(int i = 0; i < Vectors.Count; i++){
        		Gizmos.color = Color.yellow;
        		Gizmos.DrawSphere((Vector3)Vectors[i], 0.03f);
      		}//end-for
    	}//end-if
  	}//end-OnDrawFGizmos
  	
  	
	//recursive 4-connected flood-fil
  	//weights exponentially by brightness
  	Vector3 VectorFind(int Start, byte[] image, int Stack, int Cam){
    	if(pixelsFound < pixelLimit) {
      		//add weighted ray direction to total
      		Vector Dir = new Vector((float)(Start % Width), (float)(Start / Width), 0f);
      		float Weight = Mathf.Pow(((float)image[Start] - (float)brightnessThreshold), 2) + 1f;
      		Vector2 Pos = new Vector2(Dir.x, Dir.y) * Weight;
      		float Sum = Weight;
      		image[Start] = 0; //mark pixel as done to prevent redoing it
      		pixelsFound++;

      		//assimilates surrounding pixels
      		//prevents stack overflow
      		if (Stack < 5000) {
        		if ((Mathf.Floor(Start % Width) != 0) && (image[Start - 1] > brightnessThreshold)) {
          			Vector3 nPos = VectorFind(Start - 1, image, Stack + 1, Cam);
          			Pos = new Vector2(Pos.x + nPos.x, Pos.y + nPos.y);
          			Sum += nPos.z;
        		}//end-if
        
        		if ((Mathf.Floor(Start % Width) != Width - 1) && (image[Start + 1] > brightnessThreshold)) {
          			Vector3 nPos = VectorFind(Start + 1, image, Stack + 1, Cam);
          			Pos = new Vector2(Pos.x + nPos.x, Pos.y + nPos.y);
          			Sum += nPos.z;
        		}//end-if
        
        		if ((Mathf.Floor(Start / Width) != 0) && (image[Start - Width] > brightnessThreshold)) {
         			Vector3 nPos = VectorFind(Start - Width, image, Stack + 1, Cam);
          			Pos = new Vector2(Pos.x + nPos.x, Pos.y + nPos.y);
          			Sum += nPos.z;
        		}//end-if
        
        		if ((Mathf.Floor(Start / Width) != Height - 1) && (image[Start + Width] > brightnessThreshold)) {
          			Vector3 nPos = VectorFind(Start + Width, image, Stack + 1, Cam);
          			Pos = new Vector2(Pos.x + nPos.x, Pos.y + nPos.y);
          			Sum += nPos.z;
        		}//end-if
      		}//end-if
      		return new Vector3(Pos.x, Pos.y, Sum); //good
      	}//end-if
      	else{
      		return new Vector3(1f, 1f, 1f); //bad
   		}//end-else
  	}//end-VectorFind



  	//method that sets the dimensions of byte array
  	private void SetDimensions(int width, int height){
    	Debug.Log("New dimensions: " + width + " x " + height);
    	int num_pixels = width * height;
    	rightImageData = new byte[num_pixels];
    	leftImageData = new byte[num_pixels];
  	}//end-SetDimensions


  	//method that finds the distance along line that is closest to other line
  	public static float ClosestTimeOfApproach(Vector3 pos1, Vector3 vel1, Vector3 pos2, Vector3 vel2){
    	Vector3 dv = vel1 - vel2;
    	float dv2 = Vector3.Dot(dv, dv);
    
   		//if tracks are almost parallel
    	if (dv2 < 0.0000001f) {
      		return 0.0f; //use time 0
    	}//end-if

    	Vector3 w0 = pos1 - pos2;
    	return (-Vector3.Dot(w0, dv) / dv2);
  	}//end-ClosestTimeOfApproach


  	//method that finds the distance where lines are closest
  	public static float ClosestDistOfApproach(Vector3 pos1, Vector3 vel1, Vector3 pos2, Vector3 vel2){
    	float t = ClosestTimeOfApproach(pos1, vel1, pos2, vel2);
    	Vector3 p1 = pos1 + (t * vel1);
    	Vector3 p2 = pos2 + (t * vel2);
    	return Vector3.Distance(p1, p2); //distance at CPA
  	}


  	//method that finds the point on the line of one closest to line two
  	public static Vector3 ClosestPointOfApproach(Vector3 pos1, Vector3 vel1, Vector3 pos2, Vector3 vel2) {
    	float t = ClosestTimeOfApproach(pos1, vel1, pos2, vel2);
    
    	//only detect future approach points
    	if(t < 0){
      		return pos1;
    	}//end-if
    	
    	return (pos1 + (t * vel1));
  	}//end-ClosestPointOfApproach


  	private IEnumerator CheckImageMode() {
    	checkingImageState = true;
    	yield return new WaitForSeconds(2.0f);
    	leapProvider.GetLeapController().Config.Get<Int32>("images_mode", delegate(Int32 enabled){
      	this.imagesEnabled = enabled == 0 ? false : true;
      	checkingImageState = false;
    	});
  	}//end-IEnumerator
}//end-class