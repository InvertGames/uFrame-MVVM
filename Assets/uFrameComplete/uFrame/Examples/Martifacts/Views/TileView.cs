using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public abstract partial class TileView {
    //public override string Identifier
    //{
    //    get
    //    {
    //        return Convert.ToInt32(this.transform.position.x / 10f).ToString() + Convert.ToInt32(this.transform.position.z / 10f).ToString();
    //    }
    //}

    //public override int GetHashCode()
    //{
    //    return Convert.ToInt32(this.transform.position.x / 10f) * Convert.ToInt32(this.transform.position.z / 10f);
    //}

    protected override void InitializeViewModel(ViewModel viewModel)
    {
        base.InitializeViewModel(viewModel);
        var vm = (TileViewModel) viewModel;
        vm.TileX = Convert.ToInt32(this.transform.position.x/10f);
        vm.TileY = Convert.ToInt32(this.transform.position.z/10f);
        
    }

    public override void Bind() {
        base.Bind();
        
    }
}
