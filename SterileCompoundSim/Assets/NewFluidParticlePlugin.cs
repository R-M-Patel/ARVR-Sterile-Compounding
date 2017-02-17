using UnityEngine;
using Thinksquirrel.Fluvio;
using Thinksquirrel.Fluvio.Plugins;

public class NewFluidParticlePlugin : FluidParticlePlugin
{
    protected override void OnEnablePlugin()
    {
        // Set the plugin's compute shader and kernel here.
        // Compute shader files can be found in the Fluvio-ProjectSettings/Resources/CustomPlugins folder by default.
        SetComputeShader(FluvioComputeShader.Find("CustomPlugins/NewFluidParticlePlugin 1"), "OnUpdatePlugin");
    }
    protected override void OnSetComputeShaderVariables()
    {
        // Set compute shader variables here.

        // float myFloat = 10.0f;
        // Vector4[] myBuffer = new Vector4[10];
        // SetComputePluginValue(0, myFloat);
        // SetComputePluginBuffer(1, myBuffer, true);
    }
    protected override void OnUpdatePlugin(SolverData solverData, int particleIndex)
    {
        // Main C# plugin code goes here. This runs on multiple threads, so most Unity API calls are not allowed.
    }
}