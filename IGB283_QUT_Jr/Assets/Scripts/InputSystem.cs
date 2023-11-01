using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSystem", menuName = "ScriptableObjects/New Input")]
public class InputSystem : ScriptableObject
{
   public KeyCode jumpUp;
   public KeyCode jumpForward;
   public KeyCode moveRight;
   public KeyCode moveLeft;
   public KeyCode dieTemporarily;
}
