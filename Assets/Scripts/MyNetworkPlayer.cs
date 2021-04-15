using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleDisplayNamUpdated))]
    [SerializeField] private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField] private Color displayColor = Color.black;

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newColor)
    {
        displayColor = newColor;
    }

    #endregion

    #region Command
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        // We would do server validation here
        if (newDisplayName.Length < 2 || newDisplayName.Length > 12)
            return;

        // Maybe add a function to tell the player that the name is invalid


        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client
    // Callback for changing the player's name
    private void HandleDisplayNamUpdated(string oldDisplayName, string newDisplayName)
    {
        displayNameText.text = newDisplayName;
    }

    // Callback for changing the player's color
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}
