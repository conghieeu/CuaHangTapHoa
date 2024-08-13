using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }

        public bool Interact(Interactor interactor);
    }
}
