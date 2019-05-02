using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using CLPLib.GLObject;

//glControl = new GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));

namespace CLPLib
{
    public partial class ContainerViewer : Form
    {
        bool loaded = false;//gl에서 사용 http://blog.danggun.net/3062

        private float _initialAngleX = 0;//
        private float _initialAngleY = 0;//
        private float _initialAngleZ= 0;//
        private float _zoomFactor = 0.0f; //줌팩터 값 
        private float _mouseStartX = 0.0f;
        private float _mouseStartY = 0.0f;
        private float _mouseStartZ = 0.0f;

        private List<LoadedBox> _listBoxLoaded = null; //리스트의 박스를 화면에 표시한다.
       
        

        public List<LoadedBox> ListBoxLoaded
        {
            get
            {
                return _listBoxLoaded;
            }

            set
            {
                _listBoxLoaded = value;
                //표시할 리스트를 업데이트
                LoadedBox[] list = new LoadedBox[value.Count];
                value.CopyTo(list);
                _listBoxToShow = new List<LoadedBox>(list);
            }
        }

        private List<LoadedBox> _listBoxToShow; //표시할 목록

        public ContainerViewer()
        {
            InitializeComponent();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            //glControl.Dispose();
            //glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 0, 8));
            
            //glControl.CreateControl();
            this.loaded = true;
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (false == this.loaded)
            {
                return;
            }
            
            glControl.MakeCurrent();
            GL.ClearColor(Color.White);

            //First Clear Buffers
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            
            GL.Enable(EnableCap.DepthTest); //Enable correct Z Drawings
            GL.DepthFunc(DepthFunction.Lequal); //Enable correst Z Drawings
            
            //Basic Setup for viewing
            // Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, 4 / 3, 1, 10000); //Setup Perspective

            float aspect = (float)glControl.Size.Width / (float)glControl.Size.Height;
            //aspect = aspect < 1 ? 1 : aspect;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 100, 9000);// 10 * Cube.ZoomFactor.MinSizeFactorToFit); //Setup Perspective

            Matrix4 lookat = Matrix4.LookAt(2000,0, 0,   -9000, 0, 0,      0, 1, 0);    // 계산해서 넣어야함

            GL.MatrixMode(MatrixMode.Projection); //load perspective
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, glControl.Width, glControl.Height); //Size of window

            // Improve visual quality at the expense of performance
            bool HighQuality = true;
            if (HighQuality)
            {
                int max_size;
                GL.GetInteger(GetPName.PointSizeMax, out max_size);
                GL.Enable(EnableCap.PointSmooth);
            }


            GL.Enable(EnableCap.DepthTest); //Enable correct Z Drawings
            GL.DepthFunc(DepthFunction.Less); //Enable correst Z Drawings

            GL.Rotate(_initialAngleX, Vector3d.UnitX);
            GL.Rotate(_initialAngleY, Vector3d.UnitY);
            GL.Rotate(_initialAngleZ, Vector3d.UnitZ);

            //파렛트 그리기
            //new Pallet(2000, 1000, 0, 1000);

            //if (ListBoxLoaded != null)
            //{
            //    for (int i = 0; i < ListBoxLoaded.Count; ++i)
            //    {
            //        LoadedBox boxLoaded = ListBoxLoaded[i];
            //        Cube cube = new Cube(boxLoaded);
            //     }
            //}

            if (_listBoxToShow != null)
            {
                for (int i = 0; i < _listBoxToShow.Count; ++i)
                {
                    LoadedBox boxLoaded = _listBoxToShow[i];
                    Cube cube = new Cube(boxLoaded);
                }
            }

            glControl.SwapBuffers();
            
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ev = (e as System.Windows.Forms.MouseEventArgs);
            if (ev.Button == MouseButtons.Right)
            {
                _initialAngleX += (e.Y - _mouseStartX);
                glControl.Invalidate();
            }
            if (ev.Button == MouseButtons.Left)
            {
                _initialAngleY += (e.X - _mouseStartY);
                glControl.Invalidate();
            }
            if (ev.Button == MouseButtons.Middle)
            {
                _initialAngleZ += -(e.Y - _mouseStartZ);
                glControl.Invalidate();
            }

            _mouseStartX = ev.Y;
            _mouseStartY = ev.X;
            _mouseStartZ = ev.Y;
        }

        private void glControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                _zoomFactor += 30.0f;
            }
            else
            {
                _zoomFactor -= 30.0f;
            }
            glControl.Invalidate();
        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            //표시할 박스 목록의 증감 처리
            if (e.KeyCode == Keys.Left)
            {
                //표시 대상 하나 제거
                if (_listBoxToShow.Count > 0)
                {
                    _listBoxToShow.RemoveAt(_listBoxToShow.Count - 1);
                    glControl.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                //표시 대상 하나 추가
                if (_listBoxToShow.Count < _listBoxLoaded.Count)
                {
                    _listBoxToShow.Add(_listBoxLoaded[_listBoxToShow.Count]);
                    glControl.Invalidate();
                }
                    
            }
        }
    }
}
