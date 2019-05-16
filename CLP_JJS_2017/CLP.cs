using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace CLPLib
{
    public class CLP
    {
        public static readonly bool ___ISWEIGHTCONSIEDERD = false; //false이면 중량 고려X, true이면 중량에 따른 배치고려
        private Size _CG = new Size(0,0,0);

        public Size CG
        {
            get
            {
                return _CG;
            }

            set
            {
                _CG = value;
            }
        }

        

        private double _LoadedVolume = 0f;
        public double LoadedVolume
        {
            get
            {
                return _LoadedVolume;
            }

            set
            {
                _LoadedVolume = value;
            }
        }

        private double _LoadedWeight = 0f;
        public double LoadedWeight
        {
            get
            {
                return _LoadedWeight;
            }

            set
            {
                _LoadedWeight = value;
            }
        }

        public CLP()
        {
          
        }




        //9/7임시
        public List<LoadedBox> solveCLP(int[] city)
        {
            List<LoadedBox> initial_solution = new List<LoadedBox>(); //해는 List<LoadedBox>형식으로 표현함

            //컨테이너
            Container container = new Container();
            //container.Size = new Size(11554f, 2286f, 2216f); // X, Y, Z = L, H, W CBM = 58.5
            container.Size = new Size(1000f, 500f, 800f); // X, Y, Z = L, H, W CBM = 58.5

            

            //주문정보를 읽음
            //List<BoxToLoad> listBoxToLoad = GetBoxListToLoad(); //적재 대상 박스 목록 :순열 
            List<BoxToLoad> listBoxToLoad = GetBoxListToLoad(city); //적재 대상 박스 목록 :순열 

            //적재된 박스목록
            List<LoadedBox> listLoadedBox = new List<LoadedBox>();

            //적재 공간
            List<SpaceInContainer> listSpaceInContainer = new List<SpaceInContainer>();

            //적재된 아이템이 들어있는 공간, 적재 이후에 본 공간과 하위 3개 공간으로 분할됨
            List<SpaceInContainer> listLoadedSpace = new List<SpaceInContainer>();

            //초기 적재공간으로 컨테이너 전체 세팅
            SpaceInContainer space = new SpaceInContainer();
            space.PositionCoord = new Coordinate(0, 0, 0);
            space.Size = container.Size;

            listSpaceInContainer.Add(space);//시작할 때 컨테이너 전체를 적재 공간으로 설정
            
            int idxSpace = 0;

            while (idxSpace < listSpaceInContainer.Count) //전체 스페이스를 탐색하는 동안 계산 진행
            {
                SpaceInContainer spaceToLoad = listSpaceInContainer[idxSpace];


                for (int i = 0; i < listBoxToLoad.Count; ++i)
                {
                    BoxToLoad boxToLoad = listBoxToLoad[i];
                    
                    if (___ISWEIGHTCONSIEDERD == true)
                        TryToLoad_GroupApply(boxToLoad, ref spaceToLoad, container); //가용한 중량 그룹을 고려하여 적재
                    else
                        TryToLoad_NoGroup(boxToLoad, ref spaceToLoad); //가용한 공간에 적재 

                    if (spaceToLoad.LoadedBox != null)
                    {
                        /****************************************************
                        적재 성공시~
                        1.박스를 적재된 목록으로 이동(적재대상 목록에서 삭제)
                        2.적재된 공간의 하위 공간 3개를 생성하여 목록에 추가
                        *****************************************************/
                        //1.
                        LoadedBox loadedBox = spaceToLoad.LoadedBox; //TryToLoad 성공시 loadedbox가 세팅되어있음

                        listLoadedBox.Add(loadedBox); //적재된 목록에 넣고
                        listBoxToLoad.RemoveAt(i);    //적재할 목록에서 제거

                        spaceToLoad.PositionCoord = loadedBox.Coordinate;
                        spaceToLoad.Size = loadedBox.LoadedOrientation == 1 ? new Size(loadedBox.Size) : new Size(loadedBox.Size.Z, loadedBox.Size.Y, loadedBox.Size.X);

                        listLoadedSpace.Add(spaceToLoad); //적재된 박스를 공간 목록에 저장

                        //2.
                        SpaceInContainer top = GetAvailableSpaceTop(container, loadedBox);
                        SpaceInContainer wall = GetAvailableSpaceWall(container, loadedBox, listLoadedSpace); //z방향 남는 공간
                        SpaceInContainer door = GetAvailableSpaceDoor(container, loadedBox, listLoadedSpace); //x방향 남는 공간

                        listSpaceInContainer.RemoveAt(idxSpace);

                        if (door != null)
                            listSpaceInContainer.Insert(idxSpace, door);
                        if (wall != null)
                            listSpaceInContainer.Insert(idxSpace, wall);
                        if (top != null)
                            listSpaceInContainer.Insert(idxSpace, top);

                        idxSpace = -1;

                        break; //적재 성공시 박스 처리 후 빠져나감
                    }
                }
                ++idxSpace; //다음 Space 탐색
            }


            for (int i = 0; i < listLoadedBox.Count; ++i)
            {
                LoadedBox boxLoaded = listLoadedBox[i];
                System.Diagnostics.Debug.WriteLine("적재된 박스목록");
            }



            //총 적재 부피
            double loadVolume = 0;
            //무게중심Xcm = ( m1 * x1 + m2 * x2 ) / ( m1 + m2 )
            float wX = (float)container.TareWeight * container.Size.X;
            float wY = (float)container.TareWeight * container.Size.Y;
            float wZ = (float)container.TareWeight * container.Size.Z;
            //총 무게
            float weightSum = (float)container.TareWeight;

            for (int i = 0; i < listLoadedBox.Count; ++i)
            {
                LoadedBox loadedBox = listLoadedBox[i];
                wX += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.X + loadedBox.Coordinate.X) / 2f ;
                wY += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.Y + loadedBox.Coordinate.Y) / 2f;
                wZ += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.Z + loadedBox.Coordinate.Z) / 2f;
                weightSum += (float)loadedBox.Weight;
                
                loadVolume += loadedBox.Size.X * loadedBox.Size.Y * loadedBox.Size.Z;
            }

            //적재된 무게 중심
            this.CG.X = wX / weightSum;
            this.CG.Y = wY / weightSum;
            this.CG.Z = wZ / weightSum;
            
            this.LoadedVolume = loadVolume;
            this.LoadedWeight = weightSum;

            //결과 뷰어
            //ContainerViewer cv = new ContainerViewer();
            //cv.ListBoxLoaded = listLoadedBox; //적재된 박스를 뷰어에 세팅
            //cv.ShowDialog();
            //cv.Show();

            if (listBoxToLoad.Count > 0)
                System.Diagnostics.Debug.WriteLine("남은 박스 있음");
            else
                System.Diagnostics.Debug.WriteLine("모든 박스 적재");

            return listLoadedBox;
        }



        //public void solveCLP()
        //{
        //    List<LoadedBox> initial_solution = new List<LoadedBox>(); //해는 List<LoadedBox>형식으로 표현함

        //    //컨테이너
        //    Container container = new Container();
        //    //container.Size = new Size(11554f, 2286f, 2216f); // X, Y, Z = L, H, W CBM = 58.5
        //    container.Size = new Size(1000f, 500f, 800f); // X, Y, Z = L, H, W CBM = 58.5

        //    //주문정보를 읽음
        //    List<BoxToLoad> listBoxToLoad = GetBoxListToLoad(); //적재 대상 박스 목록 :순열 

        //    //적재된 박스목록
        //    List<LoadedBox> listLoadedBox = new List<LoadedBox>();

        //    //적재 공간
        //    List<SpaceInContainer> listSpaceInContainer = new List<SpaceInContainer>();

        //    //적재된 아이템이 들어있는 공간, 적재 이후에 본 공간과 하위 3개 공간으로 분할됨
        //    List<SpaceInContainer> listLoadedSpace = new List<SpaceInContainer>();

        //    //초기 적재공간으로 컨테이너 전체 세팅
        //    SpaceInContainer space = new SpaceInContainer();
        //    space.Coordinate = new Coordinate(0, 0, 0);
        //    space.Size = container.Size;

        //    listSpaceInContainer.Add(space);//시작할 때 컨테이너 전체를 적재 공간으로 설정


        //    int idxSpace = 0;

        //    while (idxSpace < listSpaceInContainer.Count) //전체 스페이스를 탐색하는 동안 계산 진행
        //    {
        //        SpaceInContainer spaceToLoad = listSpaceInContainer[idxSpace];


        //        for (int i = 0; i < listBoxToLoad.Count; ++i)
        //        {
        //            BoxToLoad boxToLoad = listBoxToLoad[i];

        //            TryToLoad(boxToLoad, ref spaceToLoad); //가용한 공간에 적재 

        //            if (spaceToLoad.LoadedBox != null)
        //            {
        //                /****************************************************
        //                적재 성공시~
        //                1.박스를 적재된 목록으로 이동(적재대상 목록에서 삭제)
        //                2.적재된 공간의 하위 공간 3개를 생성하여 목록에 추가
        //                *****************************************************/
        //                //1.
        //                LoadedBox loadedBox = spaceToLoad.LoadedBox; //TryToLoad 성공시 loadedbox가 세팅되어있음

        //                listLoadedBox.Add(loadedBox); //적재된 목록에 넣고
        //                listBoxToLoad.RemoveAt(i);    //적재할 목록에서 제거

        //                //spaceToLoad.Coordinate = loadedBox.Coordinate;
        //                spaceToLoad.Size = loadedBox.LoadedOrientation == 1 ?  new Size(loadedBox.Size) : new Size(loadedBox.Size.Z, loadedBox.Size.Y, loadedBox.Size.X);

        //                listLoadedSpace.Add(spaceToLoad); //적재된 박스를 공간 목록에 저장

        //                //2.
        //                SpaceInContainer top = GetAvailableSpaceTop(container, loadedBox);
        //                SpaceInContainer wall = GetAvailableSpaceWall(container, loadedBox, listLoadedSpace); //z방향 남는 공간
        //                SpaceInContainer door = GetAvailableSpaceDoor(container, loadedBox, listLoadedSpace); //x방향 남는 공간

        //                listSpaceInContainer.RemoveAt(idxSpace);


        //                if (door != null)
        //                    listSpaceInContainer.Insert(idxSpace, door);
        //                if (wall != null)
        //                    listSpaceInContainer.Insert(idxSpace, wall);
        //                if (top != null)
        //                    listSpaceInContainer.Insert(idxSpace, top);

        //                idxSpace = -1;

        //                break; //적재 성공시 박스 처리 후 빠져나감
        //            }
        //        }
        //        ++idxSpace; //다음 Space 탐색
        //    }


        //    for (int i = 0; i < listLoadedBox.Count; ++i)
        //    {
        //        LoadedBox boxLoaded = listLoadedBox[i];
        //        System.Diagnostics.Debug.WriteLine("적재된 박스목록");
        //    }

        //    //총 적재 부피
        //    double loadVolume = 0;
        //    //무게중심Xcm = ( m1 * x1 + m2 * x2 ) / ( m1 + m2 )
        //    float wX = (float)container.TareWeight * container.Size.X;
        //    float wY = (float)container.TareWeight * container.Size.Y;
        //    float wZ = (float)container.TareWeight * container.Size.Z;
        //    //총 무게
        //    float weightSum = (float)container.TareWeight;

        //    for (int i = 0; i < listLoadedBox.Count; ++i)
        //    {
        //        LoadedBox loadedBox = listLoadedBox[i];
        //        wX += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.X + loadedBox.Coordinate.X) / 2f;
        //        wY += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.Y + loadedBox.Coordinate.Y) / 2f;
        //        wZ += (float)loadedBox.Weight * (loadedBox.CoordinateEnd.Z + loadedBox.Coordinate.Z) / 2f;
        //        weightSum += (float)loadedBox.Weight;

        //        loadVolume += loadedBox.Size.X * loadedBox.Size.Y * loadedBox.Size.Z;
        //    }

        //    //적재된 무게 중심
        //    Size cg = new Size(0, 0, 0);
        //    cg.X = wX / weightSum;
        //    cg.Y = wY / weightSum;
        //    cg.Z = wZ / weightSum;
        //    this.CG = cg;
        //    this.LoadedVolume = loadVolume;


        //    //결과 뷰어
        //    ContainerViewer cv = new ContainerViewer();
        //    cv.ListBoxLoaded = listLoadedBox; //적재된 박스를 뷰어에 세팅
        //    cv.ShowDialog();

        //    if (listBoxToLoad.Count > 0)
        //        System.Diagnostics.Debug.WriteLine("남은 박스 있음");
        //    else
        //        System.Diagnostics.Debug.WriteLine("모든 박스 적재");
        //}


        /// <summary>
        /// 중량 그룹과 상관 없이 적재
        /// </summary>
        /// <param name="boxToLoad"></param>
        /// <param name="space"></param>
        private static void TryToLoad_NoGroup(BoxToLoad boxToLoad, ref SpaceInContainer space)
        {
            //space에 boxToLoad를 적재한다. 

            if (space.Size.X >= boxToLoad.Size.Z && space.Size.Z >= boxToLoad.Size.X && space.Size.Y >= boxToLoad.Size.Y) //방향2
            {

                //그룹 위치가 매칭되면 적용
                //위치 비교 
                LoadedBox boxLoaded = new LoadedBox(boxToLoad);
                boxLoaded.LoadedOrientation = 2;
                boxLoaded.Coordinate = new Size(space.PositionCoord.X, space.PositionCoord.Y, space.PositionCoord.Z);  //좌표는 공간과 동일
                space.LoadedBox = boxLoaded;
                return;
            }

            if (space.Size.X >= boxToLoad.Size.X && space.Size.Z >= boxToLoad.Size.Z && space.Size.Y >= boxToLoad.Size.Y) //방향1
            {
                LoadedBox boxLoaded = new LoadedBox(boxToLoad);
                boxLoaded.LoadedOrientation = 1;
                boxLoaded.Coordinate = new Size(space.PositionCoord.X, space.PositionCoord.Y, space.PositionCoord.Z);  //좌표는 공간과 동일
                space.LoadedBox = boxLoaded;
                return;
            }

        }


        /// <summary>
        /// 중량 그룹을 고려하여 적재(작성중)
        /// </summary>
        /// <param name="boxToLoad"></param>
        /// <param name="space"></param>
        private static void TryToLoad_GroupApply(BoxToLoad boxToLoad, ref SpaceInContainer space, Container container)
        {
            //space에 boxToLoad를 적재한다. 
            
            if (space.Size.X >= boxToLoad.Size.Z && space.Size.Z >= boxToLoad.Size.X && space.Size.Y >= boxToLoad.Size.Y ) //방향2
            {

                //그룹 위치가 매칭되면 적용

                //위치 비교 
                //박스의 중앙부분과 컨테이너의 위치로 AreaGroup 계산하기
                //회전한 박스의 컨테이너내 좌표
                float x = space.PositionCoord.X + boxToLoad.Size.Z;
                float z = space.PositionCoord.Z + boxToLoad.Size.X;
                string area = container.GetAreaGroup(new Size(x, 0, z));
                if (area.Equals(boxToLoad.WEIGHT_RANK_GROUP) == true)
                {
                    LoadedBox boxLoaded = new LoadedBox(boxToLoad);
                    boxLoaded.LoadedOrientation = 2;
                    boxLoaded.Coordinate = new Size(space.PositionCoord.X, space.PositionCoord.Y, space.PositionCoord.Z);  //좌표는 공간과 동일
                    space.LoadedBox = boxLoaded;
                    return;
                }
            }

            if (space.Size.X >= boxToLoad.Size.X && space.Size.Z >= boxToLoad.Size.Z && space.Size.Y >= boxToLoad.Size.Y) //방향1
            {
                //위치 비교 
                //박스의 중앙부분과 컨테이너의 위치로 AreaGroup 계산하기
                //회전하지 않은 박스의 컨테이너내 좌표
                float x = space.PositionCoord.X + boxToLoad.Size.X;
                float z = space.PositionCoord.Z + boxToLoad.Size.Z;
                string area = container.GetAreaGroup(new Size(x, 0, z));
                if (area.Equals(boxToLoad.WEIGHT_RANK_GROUP) == true)
                {
                    LoadedBox boxLoaded = new LoadedBox(boxToLoad);
                    boxLoaded.LoadedOrientation = 1;
                    boxLoaded.Coordinate = new Size(space.PositionCoord.X, space.PositionCoord.Y, space.PositionCoord.Z);  //좌표는 공간과 동일
                    space.LoadedBox = boxLoaded;
                    return;
                }
            }

        }


        
        private List<BoxToLoad> GetBoxListToLoad(int[] city)
        {
            string fileName = "boxToLoad.csv";
            StreamReader sr = new StreamReader(fileName, Encoding.ASCII);
            SortedList<string, BoxToLoad> sortedList = new SortedList<string, BoxToLoad>();
            
            int rowNum = 0;
            while (sr.Peek() > 0)
            {
                string line = sr.ReadLine();
                ++rowNum;
                if (rowNum == 1)
                    continue;
                
                System.Diagnostics.Debug.WriteLine(line);
                
                BoxToLoad boxToLoad = new BoxToLoad();
                string[] cols = line.Split(',');
                boxToLoad.Name = cols[0];
                Size size = new Size(Convert.ToSingle(cols[1]), Convert.ToSingle(cols[3]), Convert.ToSingle(cols[2]));//width, length, height 오른손좌표
                boxToLoad.Size = size;
                boxToLoad.Weight = Convert.ToDouble(cols[4]);
                boxToLoad.CountToLoad = Convert.ToInt32(cols[5]);
                boxToLoad.LoadOrientation = 3;
                //int rank = Convert.ToInt32(cols[6]); //rank
                boxToLoad.WEIGHT_RANK_GROUP = cols[8];//3등분하여 A,B,C - A가 가장 무거움

                if (boxToLoad.BoxColor == 0)
                {
                    Random rand = new Random();
                    byte[] colorBytes = new byte[3];
                    rand.NextBytes(colorBytes);
                    System.Threading.Thread.Sleep(100);
                    Color tmpColor = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                    int boxColor = (Convert.ToInt32(tmpColor.B) << 16) + (Convert.ToInt32(tmpColor.G) << 8) + (Convert.ToInt32(tmpColor.R));
                    boxToLoad.BoxColor = boxColor;

                }

                //개수만큼 증가
                int countToLoad = boxToLoad.CountToLoad;
                try
                {
                    for (int i = 0; i < countToLoad; ++i)
                    {
                        boxToLoad = (BoxToLoad)boxToLoad.Clone();
                        boxToLoad.CountToLoad = 1;
                        string key = city[rowNum - 2].ToString("00000") + i.ToString("0000");
                        sortedList.Add(key, boxToLoad);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            sr.Close();
            
            return sortedList.Values.ToList<BoxToLoad>();
        }


        /// <summary>
        /// 적재할 박스 목록을, 파일로부터 읽어들인다.
        /// </summary>
        /// <returns></returns>
        private List<BoxToLoad> GetBoxListToLoad()
        {
            string fileName = "boxToLoad.csv";
            StreamReader sr = new StreamReader(fileName, Encoding.ASCII);
            List<BoxToLoad> list = new List<BoxToLoad>();

            int rowNum = 0;
            while (sr.Peek() > 0)
            {
                string line = sr.ReadLine();
                ++rowNum;
                if (rowNum == 1)
                    continue;
                
                System.Diagnostics.Debug.WriteLine(line);
                
                BoxToLoad boxToLoad = new BoxToLoad();
                string[] cols = line.Split(',');
                boxToLoad.Name = cols[0];
                Size size = new Size(Convert.ToSingle(cols[1]), Convert.ToSingle(cols[3]), Convert.ToSingle(cols[2]));//width, length, height 오른손좌표
                boxToLoad.Size = size; 
                boxToLoad.Weight = Convert.ToDouble(cols[4]);
                boxToLoad.CountToLoad = Convert.ToInt32(cols[5]);
                boxToLoad.LoadOrientation = 3;

                if (boxToLoad.BoxColor == 0)
                {
                    Random rand = new Random();
                    byte[] colorBytes = new byte[3];
                    rand.NextBytes(colorBytes);
                    System.Threading.Thread.Sleep(100);
                    Color tmpColor = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                    int boxColor = (Convert.ToInt32(tmpColor.B) << 16) + (Convert.ToInt32(tmpColor.G) << 8) + (Convert.ToInt32(tmpColor.R));
                    boxToLoad.BoxColor = boxColor;

                }

                //개수만큼 증가
                int countToLoad = boxToLoad.CountToLoad;
                for (int i = 0; i < countToLoad; ++i)
                {
                    boxToLoad = (BoxToLoad)boxToLoad.Clone();
                    boxToLoad.CountToLoad = 1;
                    list.Add(boxToLoad);
                }
                //list.Add(boxToLoad);
            }
            sr.Close();
            
            return list;
        }


        //1. 이번에 쌓은 박스의 상단 적재가능공간
        private SpaceInContainer GetAvailableSpaceTop(Container container, LoadedBox loadedBox)
        {
            if (container == null)
                return null;

            if (loadedBox == null)
                return null;

            
            SpaceInContainer spaceTop = new SpaceInContainer();
            spaceTop.PositionCoord.X = loadedBox.Coordinate.X;
            spaceTop.PositionCoord.Y = loadedBox.CoordinateEnd.Y;
            spaceTop.PositionCoord.Z = loadedBox.Coordinate.Z;
            
            spaceTop.Size.X = loadedBox.CoordinateEnd.X - loadedBox.Coordinate.X;
            spaceTop.Size.Y = container.Size.Y - loadedBox.CoordinateEnd.Y; //적재된 박스 윗면에서 컨테이너 천정까지
            spaceTop.Size.Z = loadedBox.CoordinateEnd.Z - loadedBox.Coordinate.Z; 
            
            return spaceTop;
        }

        // 이번에 쌓은 박스의 벽면방향 적재가능공간
        private SpaceInContainer GetAvailableSpaceWall(Container container, LoadedBox loadedBox, List<SpaceInContainer> listLoadedSpace)
        {
            if (container == null)
                return null;

            if (loadedBox == null)
                return null;
            
            SpaceInContainer spaceWall = new SpaceInContainer();

            //2.박스에서 width쪽 벽면까지
            spaceWall.PositionCoord.X = loadedBox.Coordinate.X;
            spaceWall.PositionCoord.Y = loadedBox.Coordinate.Y;
            spaceWall.PositionCoord.Z = loadedBox.CoordinateEnd.Z;

            spaceWall.Size.X = loadedBox.CoordinateEnd.X - loadedBox.Coordinate.X;
            spaceWall.Size.Y = container.Size.Y - loadedBox.Coordinate.Y;

            if (spaceWall.PositionCoord.Y.Equals(0.0f))
            {   //바닥이면 
                spaceWall.Size.Z = container.Size.Z - loadedBox.CoordinateEnd.Z;
            }
            else
            {   //바닥이 아니고, 하단에 다른 박스가 있으면
                //하단에 지지되는 부분이 없다면 지지되는 곳까지만 적재 ****오류 예상 2017.10.25
                spaceWall.Size.Z = 0;
                for (int i = 0; i < listLoadedSpace.Count - 1; ++i) 
                {
                    SpaceInContainer spaceCheck = listLoadedSpace[i];
                    if (spaceCheck.LoadedBox.CoordinateEnd.Y == spaceWall.PositionCoord.Y //공간의 윗면이 찾는 공간의 바닥과 같고 하단에 있으면, 
                        && spaceWall.PositionCoord.X < spaceCheck.LoadedBox.CoordinateEnd.X
                        && spaceWall.PositionCoord.Z < spaceCheck.LoadedBox.CoordinateEnd.Z
                        )
                    {
                        float z = spaceCheck.LoadedBox.CoordinateEnd.Z - spaceWall.PositionCoord.Z;
                        spaceWall.Size.Z = z > spaceWall.Size.Z ? z : spaceWall.Size.Z;
                    }
                    
                }

                if (spaceWall.Size.Z == 0)
                {
                    //받침 공간이 없으면!
                    return null;
                }
            }
        
           
            return spaceWall;
        }



        private SpaceInContainer GetAvailableSpaceDoor(Container container, LoadedBox loadedBox, List<SpaceInContainer> listLoadedSpace)
        {
            if (container == null)
                return null;

            if (loadedBox == null)
                return null;


            //3.뒷편 공간
            SpaceInContainer spaceDoor = new SpaceInContainer();

            spaceDoor.PositionCoord.X = loadedBox.CoordinateEnd.X;
            spaceDoor.PositionCoord.Y = loadedBox.Coordinate.Y;
            spaceDoor.PositionCoord.Z = loadedBox.Coordinate.Z;

            spaceDoor.Size.Y = container.Size.Y - loadedBox.Coordinate.Y;
            spaceDoor.Size.Z = 0;
            spaceDoor.Size.X = 0;

            if (spaceDoor.PositionCoord.Y > 0)
            {
                //하단에 지지되는 부분이 없다면 지지되는 곳까지만 적재
                for (int i = 0; i < listLoadedSpace.Count; ++i) 
                {
                    SpaceInContainer spaceCheck = listLoadedSpace[i];
                    if (spaceCheck.LoadedBox.CoordinateEnd.Y == spaceDoor.PositionCoord.Y //공간의 윗면이 찾는 공간의 바닥과 같고 하단에 있으면, 
                        && spaceDoor.PositionCoord.X < spaceCheck.LoadedBox.CoordinateEnd.X
                        && spaceDoor.PositionCoord.Z < spaceCheck.LoadedBox.CoordinateEnd.Z 
                        )
                    {
                        float z = spaceCheck.LoadedBox.CoordinateEnd.Z - spaceDoor.PositionCoord.Z;
                        spaceDoor.Size.Z = z > spaceDoor.Size.Z ? z : spaceDoor.Size.Z;

                        float x = spaceCheck.LoadedBox.CoordinateEnd.X - spaceDoor.PositionCoord.X;
                        spaceDoor.Size.X = x > spaceDoor.Size.X ? x : spaceDoor.Size.X;
                    }
                }
                if (spaceDoor.Size.X == 0)
                {
                    //받침 공간이 없으면!
                    return null;
                }

            }
            else
            {
                spaceDoor.Size.Z = container.Size.Z - loadedBox.Coordinate.Z;
                spaceDoor.Size.X = container.Size.X - loadedBox.CoordinateEnd.X;
                if (spaceDoor.PositionCoord.Z > 0) //Z=0가 아니면 X의 위치를 MAX로 제한 
                {
                    if (listLoadedSpace.Count >= 2)
                    {
                        float maxX = 0;
                        for (int kk = 0;  kk < listLoadedSpace.Count; ++kk)
                        {
                            SpaceInContainer spaceCheck = listLoadedSpace[kk];
                            maxX = maxX < spaceCheck.LoadedBox.CoordinateEnd.X ? spaceCheck.LoadedBox.CoordinateEnd.X: maxX;
                        }

                        spaceDoor.Size.X = maxX - spaceDoor.PositionCoord.X;
                    }
                }
            }




            return spaceDoor;
        }


    }
}
