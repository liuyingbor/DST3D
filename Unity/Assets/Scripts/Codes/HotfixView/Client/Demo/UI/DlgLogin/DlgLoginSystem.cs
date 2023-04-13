using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (DlgLogin))]
    public static class DlgLoginSystem
    {
        public static void RegisterUIEvent(this DlgLogin self)
        {
            self.View.E_LoginButton.AddListenerAsync(self.OnLoginClickHandler);
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {
            self.View.E_ServerAddressDropdown.options.Clear();
            
            string[] lines = { $"{ConstValue.RouterHttpHost}:{ConstValue.RouterHttpPort}", $"{ConstValue.RouterHttpHost2}:{ConstValue.RouterHttpPort}" };
            foreach (string address in lines)
            {
                Dropdown.OptionData optionData = new();
                optionData.text = address;
                self.View.E_ServerAddressDropdown.options.Add(optionData);
            }

            self.View.E_AccountInputField.text = PlayerPrefs.GetString("Account");
            self.View.E_PasswordInputField.text = PlayerPrefs.GetString("Password");
        }

        private static async ETTask OnLoginClickHandler(this DlgLogin self)
        {
            string account = self.View.E_AccountInputField.text;
            string password = self.View.E_PasswordInputField.text;
            
            int errcode = await LoginHelper.Login(self.ClientScene(), self.View.E_AccountInputField.text, self.View.E_PasswordInputField.text);
            
            if (errcode != ErrorCode.ERR_Success) return;
            PlayerPrefs.SetString("Account", account);
            PlayerPrefs.SetString("Password", password);
        }
    }
}