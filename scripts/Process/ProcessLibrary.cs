using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ProcessLibrary {

    public static ProcessFactoryRef<string, object> MessageBox = new ProcessFactoryRef<string, object>();
    public static ProcessFactoryRef<string, GameObject> GameObjectFinder = new ProcessFactoryRef<string, GameObject>();
    public static ProcessFactoryRef<ConversationArgs, object> Conversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<ConversationArgs, object> BeginConversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<ConversationArgs, object> ConversationSegment = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<ConversationArgs, object> EndConversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static ProcessFactoryRef<object, object> Explore = new ProcessFactoryRef<object, object>();

}
