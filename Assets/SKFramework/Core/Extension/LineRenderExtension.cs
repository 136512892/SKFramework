using UnityEngine;

namespace SK.Framework
{
    public static class LineRenderExtension
    {
        public static LineRenderer SetStartWidth(this LineRenderer self, float width)
        {
            self.startWidth = width;
            return self;
        }
        public static LineRenderer SetEndWidth(this LineRenderer self, float width)
        {
            self.endWidth = width;
            return self;
        }
        public static LineRenderer SetStartColor(this LineRenderer self, Color color)
        {
            self.startColor = color;
            return self;
        }
        public static LineRenderer SetEndColor(this LineRenderer self, Color color)
        {
            self.endColor = color;
            return self;
        }
        public static LineRenderer SetPositionCount(this LineRenderer self, int count)
        {
            self.positionCount = count;
            return self;
        }
        public static LineRenderer SetLinePosition(this LineRenderer self, int index, Vector3 position)
        {
            self.SetPosition(index, position);
            return self;
        }
        public static LineRenderer SetLinePositions(this LineRenderer self, Vector3[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                self.SetPosition(i, positions[i]);
            }
            return self;
        }
        public static LineRenderer SetLoop(this LineRenderer self, bool loop)
        {
            self.loop = loop;
            return self;
        }
        public static LineRenderer SetCornerVertices(this LineRenderer self, int cornerVertices)
        {
            self.numCornerVertices = cornerVertices;
            return self;
        }
        public static LineRenderer SetEndCapVertices(this LineRenderer self, int endCapVertices)
        {
            self.numCapVertices = endCapVertices;
            return self;
        }
        public static LineRenderer SetAlignment(this LineRenderer self, LineAlignment alignment)
        {
            self.alignment = alignment;
            return self;
        }
        public static LineRenderer SetTextureMode(this LineRenderer self, LineTextureMode textureMode)
        {
            self.textureMode = textureMode;
            return self;
        }
        public static LineRenderer SetMaterial(this LineRenderer self, Material material)
        {
            self.material = material;
            return self;
        }
        public static LineRenderer BakeMesh(this LineRenderer self, out Mesh mesh)
        {
            mesh = new Mesh();
            self.BakeMesh(mesh);
            return self;
        }

        /// <summary>
        /// 绘制一个圆
        /// </summary>
        /// <param name="self">LineRenderer组件</param>
        /// <param name="center">圆的中心点坐标</param>
        /// <param name="direction">圆的朝向</param>
        /// <param name="radius">圆的半径</param>
        /// <param name="thickness">圆的宽度 即LineRenderer组件width</param>
        /// <returns></returns>
        public static LineRenderer DrawCircle(this LineRenderer self, Vector3 center, Vector3 direction, float radius, float thickness)
        {
            //设置宽度
            self.startWidth = thickness;
            self.endWidth = thickness;
            //设置坐标点个数
            self.positionCount = 360;
            //设置闭环
            self.loop = true;
            //朝向
            Quaternion q = Quaternion.FromToRotation(Vector3.up, direction);
            float x, z;
            //每一度求得一个在圆上的坐标点
            for (int i = 0; i < 360; i++)
            {
                x = center.x + radius * Mathf.Cos(i * Mathf.PI / 180f);
                z = center.z + radius * Mathf.Sin(i * Mathf.PI / 180f);
                Vector3 pos = new Vector3(x, center.y, z);
                pos = q * pos;
                self.SetPosition(i, pos);
            }
            return self;
        }
    }
}