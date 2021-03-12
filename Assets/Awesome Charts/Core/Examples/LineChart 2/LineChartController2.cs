using UnityEngine;

namespace AwesomeCharts {
    public class LineChartController2 : MonoBehaviour {

        public LineChart chart;

        private void Start() {
            ConfigChart();
            AddChartData();
        }

        private void ConfigChart() {
            chart.Config.ValueIndicatorSize = 17;

            chart.XAxis.DashedLine = true;
            chart.XAxis.LineThickness = 1;
            chart.XAxis.LabelColor = Color.white;
            chart.XAxis.LabelSize = 18;

            chart.YAxis.DashedLine = true;
            chart.YAxis.LineThickness = 1;
            chart.YAxis.LabelColor = Color.white;
            chart.YAxis.LabelSize = 16;
        }

        private void AddChartData() {
            LineDataSet set1 = new LineDataSet();
            set1.AddEntry(new LineEntry(0, 85));
            set1.AddEntry(new LineEntry(30,62));
            set1.AddEntry(new LineEntry(50, 46));
            set1.AddEntry(new LineEntry(70, 31));
            set1.AddEntry(new LineEntry(90, 20));

            set1.LineColor = new Color32(54, 105, 126, 255);
            set1.FillColor = new Color32(54, 105, 126, 110);

            LineDataSet set2 = new LineDataSet();
            set2.AddEntry(new LineEntry(0, 5));
            set2.AddEntry(new LineEntry(5, 11));
            set2.AddEntry(new LineEntry(10, 19));
            set2.AddEntry(new LineEntry(15, 24));
            set2.AddEntry(new LineEntry(20, 35));
            set2.AddEntry(new LineEntry(25, 46));
            set2.AddEntry(new LineEntry(40, 52));
            set2.AddEntry(new LineEntry(55, 46));
            set2.AddEntry(new LineEntry(60, 31));
            set2.AddEntry(new LineEntry(65, 26));
            set2.AddEntry(new LineEntry(70, 19));
            set2.AddEntry(new LineEntry(75, 13));
            set2.AddEntry(new LineEntry(80, 9));
            set2.AddEntry(new LineEntry(90, 5));

            set2.LineColor = new Color32(9, 107, 67, 255);
            set2.FillColor = new Color32(9, 107, 67, 110);

            chart.GetChartData().DataSets.Add(set1);
            chart.GetChartData().DataSets.Add(set2);

            chart.SetDirty();
        }
    }
}
