using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorBlindCorrectionRenderer), PostProcessEvent.AfterStack, "Custom/ColorBlindCorrection")]
public sealed class ColorBlindCorrection : PostProcessEffectSettings
{
    [Header("1:Protanopia 2:Deuteranopia 3:Tritanopia")]
    [Range(0, 2)]
    public IntParameter mode = new IntParameter() { value = 0 };
}

public sealed class ColorBlindCorrectionRenderer : PostProcessEffectRenderer<ColorBlindCorrection>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/ColorBlindCorrection"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.mode.value);
    }
}