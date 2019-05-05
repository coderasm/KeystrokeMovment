using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollisionDetectionApp
{
  public partial class Form1 : Form
  {
    private int width = 300;
    private int height = 300;
    private int verticalLines = 5;
    private int horizontalLines = 5;
    private int verticalOffset = 40;
    private int horizontalOffset = 40;
    private double lineSpacing = 0;
    private double dotWidth = 0;
    private Pen pen = new Pen(Color.Black);
    private SolidBrush brush = new SolidBrush(Color.Black);
    private Point dotCell = new Point(0, 3);

    public Form1()
    {
      InitializeComponent();
      Size = new Size(width, height);
      CenterToScreen();
      lineSpacing = (double)(height - 2 * verticalOffset) / verticalLines;
      dotWidth = lineSpacing / 2;
    }

    private void onPaint(object sender, PaintEventArgs e)
    {
      drawGrid(e.Graphics);
    }

    private void drawGrid(Graphics graphics)
    {
      pen.Color = Color.Black;
      brush.Color = Color.Black;
      drawVerticalLines(graphics);
      drawHorizantalLines(graphics);
      drawPond(graphics);
      drawDot(graphics);
      drawLables(graphics);
    }

    private void drawVerticalLines(Graphics graphics)
    {
      for (int i = 0; i < verticalLines; i++)
      {
        var start = new Point((int)(horizontalOffset + i * lineSpacing), verticalOffset);
        var end = new Point((int)(horizontalOffset + i * lineSpacing), (int)(height - 2.125 * verticalOffset));
        graphics.DrawLine(pen, start, end);
      }
    }

    private void drawHorizantalLines(Graphics graphics)
    {
      for (int i = 0; i < horizontalLines; i++)
      {
        var start = new Point(horizontalOffset, (int)(verticalOffset + i * lineSpacing));
        var end = new Point((int)(width - 2.125 * horizontalOffset), (int)(verticalOffset + i * lineSpacing));
        graphics.DrawLine(pen, start, end);
      }
    }

    private void drawPond(Graphics graphics)
    {
      pen.Color = Color.Blue;
      brush.Color = Color.Blue;
      var xPos = (int)(horizontalOffset + lineSpacing);
      var yPos = (int)(verticalOffset + lineSpacing);
      var rect = new Rectangle(xPos, yPos, (int)(2 * lineSpacing), (int)(3 * lineSpacing));
      graphics.DrawEllipse(pen, rect);
      graphics.FillEllipse(brush, rect);
    }

    private void drawDot(Graphics graphics)
    {
      pen.Color = Color.LimeGreen;
      brush.Color = Color.LimeGreen;
      var xPos = (int)(horizontalOffset + dotCell.X * lineSpacing + (lineSpacing - dotWidth) / 2);
      var yPos = (int)(verticalOffset + dotCell.Y * lineSpacing + (lineSpacing - dotWidth) / 2);
      var rect = new Rectangle(xPos, yPos, (int)(dotWidth), (int)(dotWidth));
      graphics.DrawEllipse(pen, rect);
      graphics.FillEllipse(brush, rect);
    }

    private void onKeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Left:
        case Keys.J:
          moveDot(0, -1);
          break;
        case Keys.Right:
        case Keys.L:
          moveDot(0, 1);
          break;
        case Keys.Up:
        case Keys.I:
          moveDot(-1, 0);
          break;
        case Keys.Down:
        case Keys.K:
          moveDot(1, 0);
          break;
        default:
          break;
      }
    }

    private void moveDot(int deltaRow, int deltaColumn)
    {
      var newRow = dotCell.Y + deltaRow;
      var newColumn = dotCell.X + deltaColumn;
      var outOfBounds = newRow < 0 || newRow > horizontalLines - 2 || newColumn < 0 || newColumn > verticalLines - 2;
      var inPond = newRow > 0 && (newColumn == 1 || newColumn == 2);
      if (!outOfBounds && !inPond)
      {
        dotCell.X = newColumn;
        dotCell.Y = newRow;
        Invalidate();
      }
      else if (outOfBounds)
      {
        Console.WriteLine("Movement out of bounds.");
        MessageBox.Show(this, "Movement out of bounds.", "Warning");
      }
      else if (inPond)
      {
        Console.WriteLine("Movement into pond");
        MessageBox.Show(this, "Movement into pond.", "Warning");
      }
    }

    private void drawLables(Graphics graphics)
    {
      //start
      brush.Color = Color.Black;
      var font = new Font("Arial", (float)lineSpacing * .250F, FontStyle.Bold);
      var textX = 0F;
      var textY = verticalOffset + (float)lineSpacing * 3.25F;
      var format = new StringFormat();
      graphics.DrawString("Start", font, brush, textX, textY, format);

      //pond
      brush.Color = Color.White;
      font = new Font("Arial", (float)lineSpacing * .250F, FontStyle.Bold);
      textX = horizontalOffset + (float)lineSpacing * 1.5F;
      textY = verticalOffset + (float)lineSpacing * 2.25F;
      graphics.DrawString("Pond", font, brush, textX, textY, format);

      //end
      brush.Color = Color.Red;
      font = new Font("Arial", (float)lineSpacing * .250F, FontStyle.Bold);
      textX = horizontalOffset + (float)lineSpacing * 4F;
      textY = verticalOffset + (float)lineSpacing * 3.25F;
      graphics.DrawString("End", font, brush, textX, textY, format);
    }
  }
}
