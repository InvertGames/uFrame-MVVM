using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class RoverMoveHudView {
    public dfProgressBar _BatteryBar;
    public dfLabel _BatteryLabel;
    public dfLabel _StatusLabel;
    public dfPanel _StatusPanel;

    public dfButton _DrillButton;
    public dfButton _FlareButton;
    public dfButton _SonarButton;

    public override void Bind() {
        base.Bind();
    }

    public override void BatteryChanged(int value)
    {
        base.BatteryChanged(value);
        Debug.Log("Battery Changed");
        _BatteryBar.Value = (value/50f);
        _BatteryLabel.Text = "Battery: " + value.ToString();
    }

    public override void StateChanged(RoverState value)
    {
        base.StateChanged(value);
       
        if (value == RoverState.Idle)
        {
           
            StopAllCoroutines();
            _StatusPanel.IsVisible = false;
        }
        else
        {
            StartCoroutine(BlinkPanel());
        }
        _StatusLabel.Text = value.ToString();
        
    }

    public IEnumerator BlinkPanel()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            _StatusPanel.IsVisible = !_StatusLabel.IsVisible;
        }
        
    }
    void Update()
    {
        
    }

    public override ViewBase CreateCollectedArtifactsView(ArtifactViewModel artifact)
    {
        return null;
    }
}
