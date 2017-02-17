// ---------------------------------------------------------------------------------------
// Custom plugin properties
// ---------------------------------------------------------------------------------------

// #define FLUVIO_PLUGIN_DATA_0 float
// #define FLUVIO_PLUGIN_DATA_RW_1 float4

// ---------------------------------------------------------------------------------------
// Main include
// ---------------------------------------------------------------------------------------

// If the location of this shader or FluvioCompute.cginc is changed,
// change this to the RELATIVE path to the new include.
# include "../../../../Fluvio/Resources/ComputeShaders/Includes/FluvioCompute.cginc"

// ---------------------------------------------------------------------------------------
// Main plugin
// ---------------------------------------------------------------------------------------

FLUVIO_KERNEL(OnUpdatePlugin)
{
    int particleIndex = get_global_id(0);

	if (FluvioShouldUpdatePlugin(particleIndex))
    {
        // Main plugin code goes here.
    }
}