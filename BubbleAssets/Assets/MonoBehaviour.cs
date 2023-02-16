using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

using WikiGen.Assets;

namespace BubbleAssets.Assets
{
    public sealed class MonoScript : AssetObject
    {
        public string m_Name;

        public string m_ClassName;
        public string? m_Namespace;
        public string m_AssemblyName;

        public MonoScript(AssetFileReader reader) : base(reader.File)
        {
            m_Name = reader.ReadAlignedString();

            if (reader.Version.Major > 3 || (reader.Version.Major == 3 && reader.Version.Minor >= 4)) //3.4 and up
            {
                var m_ExecutionOrder = reader.ReadInt32();
            }
            if (reader.Version.Major < 5) //5.0 down
            {
                var m_PropertiesHash = reader.ReadUInt32();
            }
            else
            {
                var m_PropertiesHash = reader.ReadBytes(16);
            }
            if (reader.Version.Major < 3) //3.0 down
            {
                var m_PathName = reader.ReadAlignedString();
            }
            m_ClassName = reader.ReadAlignedString();
            if (reader.Version.Major >= 3) //3.0 and up
            {
                m_Namespace = reader.ReadAlignedString();
            }
            m_AssemblyName = reader.ReadAlignedString();
            if (reader.Version.Major < 2018 || (reader.Version.Major == 2018 && reader.Version.Minor < 2)) //2018.2 down
            {
                var m_IsEditorScript = reader.ReadBoolean();
            }
        }
    }

    public sealed class GameObject : AssetObject
    {
        public PPtr<Component>[] m_Components;
        public string m_Name;

        //public Transform m_Transform;
        //public MeshRenderer m_MeshRenderer;
        //public MeshFilter m_MeshFilter;
        //public SkinnedMeshRenderer m_SkinnedMeshRenderer;
        //public Animator m_Animator;
        //public Animation m_Animation;

        public GameObject(AssetFileReader reader) : base(reader.File)
        {
            int m_Component_size = reader.ReadInt32();

            m_Components = new PPtr<Component>[m_Component_size];

            for (int i = 0; i < m_Component_size; i++)
            {
                if ((reader.Version.Major == 5 && reader.Version.Minor < 5) || reader.Version.Major < 5) //5.5 down
                {
                    int first = reader.ReadInt32();
                }
                m_Components[i] = reader.ReadPtr<Component>();
            }

            var m_Layer = reader.ReadInt32();

            m_Name = reader.ReadAlignedString();
        }
    }

    public abstract class Component : AssetObject
    {
        public PPtr<GameObject> m_GameObject;

        protected Component(AssetFileReader reader) : base(reader.File)
        {
            m_GameObject = reader.ReadPtr<GameObject>();
        }
    }

    public sealed class MonoBehaviour : Component
    {
        public byte m_Enabled;

        public PPtr<MonoScript> m_Script;
        public string m_Name;

        //public SerializedType Type;

        public MonoBehaviour(AssetFileReader reader, ObjectInfo obj) : base(reader)
        {
            //Type = obj.serializedType;

            m_Enabled = reader.ReadByte();
            reader.AlignStream();

            m_Script = reader.ReadPtr<MonoScript>();
            m_Name = reader.ReadAlignedString();
        }
    }
}
