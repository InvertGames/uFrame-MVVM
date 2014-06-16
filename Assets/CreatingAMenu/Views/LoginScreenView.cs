using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public partial class LoginScreenView
{
    public UIInput _UsernameInput;
    public UIInput _PasswordInput;
    public UILabel _ErrorMessageLabel;

    public override void UsernameChanged(string value)
    {
        base.UsernameChanged(value);
        _UsernameInput.value = value;
    }
    public override void PasswordChanged(string value)
    {
        base.PasswordChanged(value);
        _PasswordInput.value = value;
    }
    public override void ErrorMessageChanged(string value)
    {
        base.ErrorMessageChanged(value);
        if (string.IsNullOrEmpty(value))
        {
            _ErrorMessageLabel.text = string.Empty;
            _ErrorMessageLabel.gameObject.SetActive(false);
        }
        else
        {
            _ErrorMessageLabel.text = value;
            StartCoroutine(ShowHideErrorLabel());    
        }
    }

    public IEnumerator ShowHideErrorLabel()
    {
        _ErrorMessageLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _ErrorMessageLabel.gameObject.SetActive(false);
    }

    public void Update()
    {
        // Two way bindings
        LoginScreen.Username = _UsernameInput.value;
        LoginScreen.Password = _PasswordInput.value;
    }

}
