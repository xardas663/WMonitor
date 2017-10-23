
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace Wojtek
{
    class PlotBuilder
    {
        PlotModel plotModel;
        public LineSeries average, alert, results;
        public PlotBuilder()
        {
            plotModel = new PlotModel
            {
                Title = "Przebieg wyników w funkcji czasu dla dnia: " + Today.ToString("yyyy-MM-dd"),
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView,
                TextColor = OxyColors.Gray,
                TitleColor = OxyColors.White,
                TitleFontSize = 14,
                LegendPosition = LegendPosition.RightTop,
                IsLegendVisible = true,
                LegendTitle = "Legenda",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendTitleFontSize = 14
            };            
        }
        public void AddAxes()
        {
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Godzina",
                TitleColor = OxyColors.LightBlue,
                StringFormat = "HH:mm:ss",
                TitleFontSize = 12,
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,

            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperatura [°C]",
                TitleFontSize = 12,
                TitleColor = OxyColors.LightBlue,
                MajorGridlineStyle = OxyPlot.LineStyle.Solid
            });

        }


        public void AddLineSeries()
        {
            results = new LineSeries
            {
                Title = "Wyniki pomiarów",
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerStroke = OxyColors.White,
                Color = OxyColors.DarkCyan
            };

            average = new LineSeries
            {
                Title = "Œrednia",
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerStroke = OxyColors.White,
                Color = OxyColors.MediumVioletRed,
            };

            alert = new LineSeries
            {
                Title = "Stan krytyczny",
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerStroke = OxyColors.White,
                Color = OxyColors.Red,
            };
        }

            
        public void AddData()
        {
            int i = 0;

            foreach (var item in MResults_chart)
            {
                results.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MResults_chart[i].actualtime), MResults_chart[i].value));
                if (i == 0 || i == MResults_chart.Count - 1)
                {
                    average.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MResults_chart[i].actualtime), MAverage[0].value));
                    alert.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MResults_chart[i].actualtime), Alert));
                }
                i++;
            }

            plotModel.Series.Add(average);
            plotModel.Series.Add(results);
            plotModel.Series.Add(alert);
        }           

           
    }

}