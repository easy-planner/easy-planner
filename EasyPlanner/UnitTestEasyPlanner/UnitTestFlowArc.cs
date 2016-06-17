using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyPlanner;

namespace UnitTestEasyPlanner
{
    [TestClass]
    public class UnitTestFlowArc
    {
        //Est-il possible de créer un arc de type FlowArcs
        [TestMethod]
        public void ConstructorParamValidesTesting()
        {
            FlowNode nodeStart = new FlowNode("Start");
            FlowNode nodeEnd = new FlowNode("End");

            FlowArc arc = new FlowArc(nodeStart, nodeEnd, 2);
            Assert.IsNotNull(arc);
        }

        //Le texte renvoyé correspond
        [TestMethod]
        public void ToStringTesting()
        {
            FlowNode nodeStart = new FlowNode("Start");
            FlowNode nodeEnd = new FlowNode("End");

            FlowArc arc = new FlowArc(nodeStart, nodeEnd, 2);
            Assert.AreEqual("(Start) -> (End)", arc.ToString());
        }
    }
}
