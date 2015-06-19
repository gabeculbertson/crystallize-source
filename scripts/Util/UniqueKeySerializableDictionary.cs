﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UniqueKeySerializableDictionary<V> : SerializableDictionary<int, V> where V : ISerializableDictionaryItem<int> {

	const int StartConstant = 1000000;
	const int Increment = 10;

	public int CurrentKey { get; set; }

	public UniqueKeySerializableDictionary() : base(){
		CurrentKey = StartConstant;
	}

	public int GetNextKey(){
		var key = CurrentKey;
		CurrentKey += Increment;
		return key;
	}

}