﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneObjectGameData {

    public string Name { get; set; }

    public SceneObjectGameData() {
        Name = "";
    }

    public SceneObjectGameData(string s) {
        Name = s;
    }

}
