using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.camera;
using ChaosAge.editor;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] Building[] prefabs;

        [Header("")]
        [SerializeField] CameraController cameraController;
        [SerializeField] BuildGrid grid;

        public BuildGrid Grid { get { return grid; } }

        public Building[] Prefabs { get => prefabs; }
        public Building SelectedBuilding { get; set; }
        public bool IsPlacingBuilding
        {
            get
            {
                return cameraController.IsPlacingBuilding;
            }
            set
            {
                cameraController.IsPlacingBuilding = value;
            }
        }
    }

}
