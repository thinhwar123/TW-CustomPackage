using System;
using TW.UGUI.Core.Views;
using TW.UGUI.Shared;
using UnityEngine;

namespace TW.UGUI.Core.Controls
{
    public abstract class ControlContainerBase : ViewContainerBase
    {
        [SerializeField] private string _name;
        [SerializeField] private UnityScreenNavigatorSettings _settings;

        public string ContainerName => _name;

        public override UnityScreenNavigatorSettings Settings
        {
            get
            {
                if (_settings == false)
                {
                    _settings = UnityScreenNavigatorSettings.Instance;
                }

                return _settings;
            }

            set
            {
                if (value == false)
                    throw new ArgumentNullException(nameof(value));

                _settings = value;
            }
        }

        protected override void Awake()
        {
            InitializePool();
        }
    }
}