# Liquid Shader                                                                        
To get a better idea of how things work, check out the shader in action, applied to many of the
bottles and vials located in the main play area of the master project! 
                                                                                
                                                                                
## Adding the shader to a scene

* To create a liquid volume in the scene, attach a liquidvolume component to any object. Sphere,
cylinder, and box objects are supported directly out of the box. You can also assign the liquid to
a generic shape, which will conform the liquid to a provided mesh.
                                                                                
## Inspector Settings 

### General Settings 

* Topology: indicates the underlying geometry. Must be either a sphere, cylinder, cube, or irregular. 
Choose irrefgular for any non-primitive types.

* Detail: Specifies the detail level of the flask. Simple (do not use 3D textures), default, default without a 
flask (means fewer render passes), texture bump (adds additional texturing and bump mapping options),
and reflections.

* Depth Aware: allows objects inside the container 

### Liquid Settings 

* Level: fill level
* Color1 and 2, Scale1 and 2: Controls appearances of two liquid components, which are blended together to 
produce a final color.
* Emission, Emission brightness: Controls emission color/brightness of liquid
* Murkiness: Amount of noise
* Alpha: Global transparency of liquid, smoke, and foam
* Dither shadow: Applies dither to liquid shadow to simulate a partially transparent shadow
* Turbulence 1: Small turbulence 
* Turbulence 2: Larger turbulence. Can be mixed with turbulence 1 to create realistic movement
* Frequency: Increase to produce shorter turbulence waves when using models with scale greater than one.
Default value of one is usually fine for this.
*Sparkling intensity/amount: Adds sparkles to liquid.
* Deep obscurance: Gradient to darker colors at bottom of liquid 

### Foam Settings 

* Color and scale: basic color/resolution of effect
* Thickness: relative height of foam with respect to flask height
* Density: increase this to create a more opaque effect
* Weight: Controls the smoothness of the foam at the bottom, near the liquid
* Turbulence: Multiplier to turbulence factors of the liquid
* Visible from bottom: Enable this to allow the foam to be visible from the bottom up (if
the liquid's alpha is low)

### Smoke settings

* Color and scale
* Speed: Speed multiplier for the smoke
* Base Obscurance: darkens smoke at the bottom

### Flask Settings

* Tint: color applied to flask. For transparent containers, reduce the alpha component of this to zero. 
* Thickness: Flasks have walls. This controls their width
* Glossiness:
* Refraction blur: Blurs background to simulate light refraction

### Physics Settings 

* React to forces: Allows the turbulence of the liquid to react with external forces and movements
* Mass: Defines the mass of the liquid. A higher value increases the weight of the liquid, meaning 
it will move less
* Angular damp: Defines the internal friction of the liquid. Higher value makes liquid return to calm 
state more quickly
* Ignore gravity: Enable this to force the liquid to rotate with the flask. Enabling "React to forces,"
obviously, disables this. 

## API Information 
To use the shader, just use its class. To change any property, get a refrence to the liquidvolume component 
using the following code:

''' 
using LiquidVolumeFX;
.....
LiquidVolume liquid = <gameObject>.GetComponent<LiquidVolume>();
'''

To change current fill  level, as an example, simple do the following:

''' 
liquid.level = .65;
'''
                                                                                                                                           
