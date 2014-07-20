using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace Invert.uFrame.Editor.UnitTests
{
    [TestClass]
    public class SerializationTests
    {
        public uFrameContainer Container { get; set; }

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void TestJsonSerializer()
        {
            var subClassIdentifier = Guid.NewGuid().ToString();
            var testViewModel = new TestViewModel()
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = "TestName",
                Bool = true,
                Float = 5.55554f,
                Vector2 = new Vector2(0f,1f),
                Vector3 = new Vector3(54f,43f,32f),
                Int = 45,
                SubClass = new SubClassViewModel() { Name = "SubClassTestName", Identifier = subClassIdentifier }
            };

            var testViewModel2 = new TestViewModel()
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = "TestName",
                Bool = true,
                Float = 5.55554f,
                Vector2 = new Vector2(0f, 1f),
                Vector3 = new Vector3(54f, 43f, 32f),
                Int = 45,
                SubClass = testViewModel.SubClass
            };

            testViewModel.SubClasses.Add(testViewModel.SubClass);
            testViewModel.SubClasses.Add(testViewModel.SubClass);
            testViewModel.SubClasses.Add(testViewModel.SubClass);
            testViewModel.SubClasses.Add(testViewModel.SubClass);
            testViewModel.SubClasses.Add(testViewModel.SubClass);

            var sceneContext = new SceneContext();
            sceneContext.ViewModels.Add(testViewModel.Identifier,testViewModel);
            sceneContext.ViewModels.Add(testViewModel2.Identifier,testViewModel2);
            var stringStorage = new StringSerializerStorage();
            sceneContext.Save(stringStorage, new JsonStream(){UseReferences = true});
            Console.WriteLine(stringStorage.Result);
            sceneContext.ViewModels.Clear();
            sceneContext = new SceneContext();
            sceneContext.Load(stringStorage, new JsonStream(){UseReferences = true});
            Assert.AreEqual(2,sceneContext.ViewModels.Count);
            var vm = sceneContext.ViewModels.First().Value as TestViewModel;

            
            Assert.AreEqual(vm.SubClass,vm.SubClasses[0]);
            sceneContext.Save(stringStorage, new JsonStream(){UseReferences = true});

            Console.WriteLine(stringStorage.Result);
        }
    }
    public class SubClassViewModel : ViewModel
    {
        public string Name { get; set; }

        public override void Write(ISerializerStream stream)
        {
            base.Write(stream);
            stream.SerializeString("Name", this.Name);
        }
        public override void Read(ISerializerStream stream)
        {
            base.Read(stream);
            //Identifier = stream.DeserializeString("Identifier");
            Name = stream.DeserializeString("Name");
        }

    }
    public class TestViewModel : ViewModel
    {
        private List<SubClassViewModel> _subClasses = new List<SubClassViewModel>();
        public string Name { get; set; }
        public int Int { get; set; }
        public Vector3 Vector3 { get; set; }
        public bool Bool { get; set; }
        public Vector2 Vector2 { get; set; }
        public float Float { get; set; }
        public SubClassViewModel SubClass { get; set; }

        public List<SubClassViewModel> SubClasses
        {
            get { return _subClasses; }
            set { _subClasses = value; }
        }

        //public int Id { get; set; }

        public override void Write(ISerializerStream stream)
        {
            base.Write(stream);
            stream.SerializeString("Name", this.Name);
            stream.SerializeVector2("Vector2", this.Vector2);
            stream.SerializeVector3("Vector3", this.Vector3);
            stream.SerializeInt("Int", this.Int);
            stream.SerializeBool("Bool", this.Bool);
            stream.SerializeFloat("Float", this.Float);
            if (SubClass != null)
            stream.SerializeObject("SubClass", this.SubClass);
            stream.SerializeArray("SubClasses",SubClasses);
        }

        public override void Read(ISerializerStream stream)
        {
            base.Read(stream);
            //Identifier = stream.DeserializeString("Identifier");
            Name = stream.DeserializeString("Name");
            Int = stream.DeserializeInt("Int");
            Vector3 = stream.DeserializeVector3("Vector3");
            Bool = stream.DeserializeBool("Bool");
            Float = stream.DeserializeFloat("Float");
            Vector2 = stream.DeserializeVector2("Vector2");
            SubClass = stream.DeserializeObject<SubClassViewModel>("SubClass");
            SubClasses = stream.DeserializeObjectArray<SubClassViewModel>("SubClasses").ToList();
        }
    }
}
