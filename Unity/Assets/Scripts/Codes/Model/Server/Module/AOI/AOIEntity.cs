﻿using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class AOIEntity: Entity, IAwake<int, float3>, IDestroy
    {
        public Unit Unit => this.GetParent<Unit>();

        public int ViewDistance;

        private long cellInstanceId;

        public Cell Cell
        {
            get
            {
                return Root.Instance.Get(this.cellInstanceId) as Cell;
            }
            set
            {
                this.cellInstanceId = value.InstanceId;
            }
        }

        // 观察进入视野的Cell
        public HashSet<long> SubEnterCells = new HashSet<long>();

        // 观察离开视野的Cell
        public HashSet<long> SubLeaveCells = new HashSet<long>();
        
        // 观察进入视野的Cell
        public HashSet<long> enterHashSet = new HashSet<long>();

        // 观察离开视野的Cell
        public HashSet<long> leaveHashSet = new HashSet<long>();

        // 我看的见的Unit
        public Dictionary<long, AOIEntity> SeeUnits = new Dictionary<long, AOIEntity>();
        
        // 看见我的Unit
        public Dictionary<long, AOIEntity> BeSeeUnits = new Dictionary<long, AOIEntity>();
        
        // 我看的见的Player
        public Dictionary<long, AOIEntity> SeePlayers = new Dictionary<long, AOIEntity>();

        // 看见我的Player单独放一个Dict，用于广播
        public Dictionary<long, AOIEntity> BeSeePlayers = new Dictionary<long, AOIEntity>();
    }
}