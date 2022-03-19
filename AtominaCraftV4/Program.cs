using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AtominaCraftV4 {
    class Program {
        static void Main(string[] args) {
            AtominaCraft game = new AtominaCraft();
            try {
                game.Run();
            }
            finally {
                GLFW.Terminate();
            }


            // List<Vertex> verts = new List<Vertex>();
            // List<uint> indices = new List<uint>();
            // verts.Add(new Vertex(new Vector3f( 0,  0,  0), new Vector3f( 0,  0,  0)));
            // verts.Add(new Vertex(new Vector3f( 1,  0,  0), new Vector3f( 1,  0,  0)));
            // verts.Add(new Vertex(new Vector3f( 0,  1,  0), new Vector3f( 0,  1,  0)));
            // verts.Add(new Vertex(new Vector3f( 0,  0,  1), new Vector3f( 0,  0,  1)));
            // verts.Add(new Vertex(new Vector3f(-1,  0,  0), new Vector3f(-1,  0,  0)));
            // verts.Add(new Vertex(new Vector3f( 0, -1,  0), new Vector3f( 0, -1,  0)));
            // verts.Add(new Vertex(new Vector3f( 0,  0, -1), new Vector3f( 0,  0, -1)));
            // void add(params uint[] indexes) {
            //     foreach (uint index in indexes) {
            //         indices.Add(index);
            //     }
            // }
            // add(0, 1);
            // add(0, 2);
            // add(0, 3);
            // add(0, 4);
            // add(0, 5);
            // add(0, 6);
            // this.lines = new MeshWithIndexBuffer(verts, indices, null);
            // this.lines.beginMode = BeginMode.Lines;
        }
    }
}