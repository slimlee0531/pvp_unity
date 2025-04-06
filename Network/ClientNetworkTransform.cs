using Unity.Netcode.Components;
using UnityEngine;
namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative() //对客户端授权
        {
            return false;
        }
    }
}