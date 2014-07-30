using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class MartifactsGameView {
    
    public override void Bind() {
        base.Bind();
        _ArtifactsContainer.gameObject.SetActive(false);
    }

    public override ViewBase CreateArtifactsView(ArtifactViewModel artifact)
    {
        return null;
    }
}
