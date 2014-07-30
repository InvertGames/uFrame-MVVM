using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class ArtifactView {
    public override string Identifier
    {
        get
        {
            return "A" + Convert.ToInt32(this.transform.position.x / 10f).ToString() + Convert.ToInt32(this.transform.position.z / 10f).ToString();
        }
    }
    public override void Bind() {
        base.Bind();
    }
}
