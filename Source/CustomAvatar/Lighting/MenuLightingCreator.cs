﻿//  Beat Saber Custom Avatars - Custom player models for body presence in Beat Saber.
//  Copyright © 2018-2021  Beat Saber Custom Avatars Contributors
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using CustomAvatar.Avatar;
using UnityEngine;
using Zenject;

namespace CustomAvatar.Lighting
{
    internal class MenuLightingCreator : IInitializable, IDisposable
    {
        private readonly LightWithIdManager _lightWithIdManager;
        private readonly MenuEnvironmentManager _menuEnvironmentManager;

        private Light _light;

        public MenuLightingCreator(LightWithIdManager lightWithIdManager, MenuEnvironmentManager menuEnvironmentManager)
        {
            _lightWithIdManager = lightWithIdManager;
            _menuEnvironmentManager = menuEnvironmentManager;
        }

        public void Initialize()
        {
            var lightObject = new GameObject("Menu Light");
            Transform lightTransform = lightObject.transform;

            lightObject.transform.SetParent(_menuEnvironmentManager.transform, false);
            lightObject.transform.rotation = Quaternion.Euler(30, 180, 0);

            _light = lightObject.AddComponent<Light>();
            _light.type = LightType.Directional;
            _light.intensity = 1;
            _light.cullingMask = AvatarLayers.kAllLayersMask;
            _light.shadows = LightShadows.Soft;
            _light.shadowStrength = 1;
            _light.renderMode = LightRenderMode.ForcePixel;

            _lightWithIdManager.didChangeSomeColorsThisFrameEvent += UpdateLightColor;

            UpdateLightColor();
        }

        public void Dispose()
        {
            _lightWithIdManager.didChangeSomeColorsThisFrameEvent -= UpdateLightColor;
        }

        private void UpdateLightColor()
        {
            _light.color = DirectionalLight.lights.Aggregate(Color.black, (acc, l) => acc + l.color * l.intensity) / DirectionalLight.lights.Sum(l => l.intensity);
        }
    }
}
