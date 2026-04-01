using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VectorEditor.Figures;
using VectorEditor.Utils;

namespace VectorEditor
{
    public class Form1 : Form
    {
        private List<Figure> _figures = new List<Figure>();
        private Figure _clipboardFigure;
        private Figure _selectedFigure;
        private bool _isDragging;
        private Point _dragStartPoint;
        private Point _originalLocation;
        private bool _isResizing;
        private int _resizeMarkerIndex = -1;
        private Rectangle _originalBounds;
        private StackMemory _undoStack;
        private StackMemory _redoStack;

        private Panel panelCanvas;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem newMenuItem;
        private ToolStripMenuItem openMenuItem;
        private ToolStripMenuItem saveMenuItem;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem undoMenuItem;
        private ToolStripMenuItem redoMenuItem;
        private ToolStripMenuItem copyMenuItem;
        private ToolStripMenuItem cutMenuItem;
        private ToolStripMenuItem pasteMenuItem;
        private ToolStripMenuItem deleteMenuItem;
        private ToolStripMenuItem figuresMenu;
        private ToolStripMenuItem rectangleMenuItem;
        private ToolStripMenuItem squareMenuItem;
        private ToolStripMenuItem lShapeMenuItem;
        private ToolStripMenuItem uShapeMenuItem;
        private ToolStripMenuItem transformMenu;
        private ToolStripMenuItem flipHorizontalMenuItem;
        private ToolStripMenuItem flipVerticalMenuItem;
        private ToolStripMenuItem rotateMenuItem;
        private ToolStripMenuItem strokeMenu;
        private ToolStripMenuItem strokeColorMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton btnRectangle;
        private ToolStripButton btnSquare;
        private ToolStripButton btnLShape;
        private ToolStripButton btnUShape;
        private ToolStripButton btnCopy;
        private ToolStripButton btnCut;
        private ToolStripButton btnPaste;
        private ToolStripButton btnDelete;
        private ToolStripButton btnUndo;
        private ToolStripButton btnRedo;
        private ToolStripButton btnStrokeColor;
        private NumericUpDown numericStrokeWidth;
        private ComboBox comboStrokeStyle;
        private ColorDialog colorDialog1;

        private Type _currentFigureType = typeof(RectangleFigure);

        public Form1()
        {
            InitializeComponent();
            InitializeStacks();
            this.KeyPreview = true;
        }

        private void InitializeComponent()
        {
            this.Text = "Векторный редактор - Вариант 6";
            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // MenuStrip
            menuStrip1 = new MenuStrip();

            // File menu
            fileMenu = new ToolStripMenuItem("Файл");
            newMenuItem = new ToolStripMenuItem("Новый");
            newMenuItem.Click += NewMenuItem_Click;
            openMenuItem = new ToolStripMenuItem("Открыть");
            openMenuItem.Click += OpenMenuItem_Click;
            saveMenuItem = new ToolStripMenuItem("Сохранить");
            saveMenuItem.Click += SaveMenuItem_Click;
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { newMenuItem, openMenuItem, saveMenuItem });

            // Edit menu
            editMenu = new ToolStripMenuItem("Правка");
            undoMenuItem = new ToolStripMenuItem("Отменить");
            undoMenuItem.Click += UndoMenuItem_Click;
            redoMenuItem = new ToolStripMenuItem("Вернуть");
            redoMenuItem.Click += RedoMenuItem_Click;
            copyMenuItem = new ToolStripMenuItem("Копировать");
            copyMenuItem.Click += CopyMenuItem_Click;
            cutMenuItem = new ToolStripMenuItem("Вырезать");
            cutMenuItem.Click += CutMenuItem_Click;
            pasteMenuItem = new ToolStripMenuItem("Вставить");
            pasteMenuItem.Click += PasteMenuItem_Click;
            deleteMenuItem = new ToolStripMenuItem("Удалить");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { undoMenuItem, redoMenuItem,
                new ToolStripSeparator(), copyMenuItem, cutMenuItem, pasteMenuItem, deleteMenuItem });

            // Figures menu
            figuresMenu = new ToolStripMenuItem("Фигуры");
            rectangleMenuItem = new ToolStripMenuItem("Прямоугольник");
            rectangleMenuItem.Click += (s, e) => SetCurrentFigureType(typeof(RectangleFigure));
            squareMenuItem = new ToolStripMenuItem("Квадрат");
            squareMenuItem.Click += (s, e) => SetCurrentFigureType(typeof(SquareFigure));
            lShapeMenuItem = new ToolStripMenuItem("Г-образная");
            lShapeMenuItem.Click += (s, e) => SetCurrentFigureType(typeof(LShapeFigure));
            uShapeMenuItem = new ToolStripMenuItem("П-образная");
            uShapeMenuItem.Click += (s, e) => SetCurrentFigureType(typeof(UShapeFigure));
            figuresMenu.DropDownItems.AddRange(new ToolStripItem[] { rectangleMenuItem, squareMenuItem,
                lShapeMenuItem, uShapeMenuItem });

            // Transform menu
            transformMenu = new ToolStripMenuItem("Преобразование");
            flipHorizontalMenuItem = new ToolStripMenuItem("Отразить по горизонтали");
            flipHorizontalMenuItem.Click += FlipHorizontalMenuItem_Click;
            flipVerticalMenuItem = new ToolStripMenuItem("Отразить по вертикали");
            flipVerticalMenuItem.Click += FlipVerticalMenuItem_Click;
            rotateMenuItem = new ToolStripMenuItem("Повернуть на 90°");
            rotateMenuItem.Click += RotateMenuItem_Click;
            transformMenu.DropDownItems.AddRange(new ToolStripItem[] { flipHorizontalMenuItem,
                flipVerticalMenuItem, rotateMenuItem });

            // Stroke menu
            strokeMenu = new ToolStripMenuItem("Контур");
            strokeColorMenuItem = new ToolStripMenuItem("Цвет контура");
            strokeColorMenuItem.Click += StrokeColorMenuItem_Click;
            strokeMenu.DropDownItems.Add(strokeColorMenuItem);

            menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, figuresMenu, transformMenu, strokeMenu });

            // ToolStrip
            toolStrip1 = new ToolStrip();
            btnRectangle = new ToolStripButton("□");
            btnRectangle.ToolTipText = "Прямоугольник";
            btnRectangle.Click += (s, e) => SetCurrentFigureType(typeof(RectangleFigure));
            btnSquare = new ToolStripButton("■");
            btnSquare.ToolTipText = "Квадрат";
            btnSquare.Click += (s, e) => SetCurrentFigureType(typeof(SquareFigure));
            btnLShape = new ToolStripButton("Γ");
            btnLShape.ToolTipText = "Г-образная";
            btnLShape.Click += (s, e) => SetCurrentFigureType(typeof(LShapeFigure));
            btnUShape = new ToolStripButton("Π");
            btnUShape.ToolTipText = "П-образная";
            btnUShape.Click += (s, e) => SetCurrentFigureType(typeof(UShapeFigure));
            toolStrip1.Items.Add(btnRectangle);
            toolStrip1.Items.Add(btnSquare);
            toolStrip1.Items.Add(btnLShape);
            toolStrip1.Items.Add(btnUShape);
            toolStrip1.Items.Add(new ToolStripSeparator());
            btnCopy = new ToolStripButton("Копировать");
            btnCopy.Click += CopyMenuItem_Click;
            btnCut = new ToolStripButton("Вырезать");
            btnCut.Click += CutMenuItem_Click;
            btnPaste = new ToolStripButton("Вставить");
            btnPaste.Click += PasteMenuItem_Click;
            btnDelete = new ToolStripButton("Удалить");
            btnDelete.Click += DeleteMenuItem_Click;
            toolStrip1.Items.Add(btnCopy);
            toolStrip1.Items.Add(btnCut);
            toolStrip1.Items.Add(btnPaste);
            toolStrip1.Items.Add(btnDelete);
            toolStrip1.Items.Add(new ToolStripSeparator());
            btnUndo = new ToolStripButton("Отменить");
            btnUndo.Click += UndoMenuItem_Click;
            btnRedo = new ToolStripButton("Вернуть");
            btnRedo.Click += RedoMenuItem_Click;
            toolStrip1.Items.Add(btnUndo);
            toolStrip1.Items.Add(btnRedo);
            toolStrip1.Items.Add(new ToolStripSeparator());

            numericStrokeWidth = new NumericUpDown();
            numericStrokeWidth.Minimum = 1;
            numericStrokeWidth.Maximum = 20;
            numericStrokeWidth.Value = 2;
            numericStrokeWidth.Width = 50;
            numericStrokeWidth.ValueChanged += NumericStrokeWidth_ValueChanged;
            toolStrip1.Items.Add(new ToolStripControlHost(numericStrokeWidth));

            comboStrokeStyle = new ComboBox();
            comboStrokeStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboStrokeStyle.Items.AddRange(new object[] { "Сплошная", "Пунктирная", "Точечная" });
            comboStrokeStyle.SelectedIndex = 0;
            comboStrokeStyle.Width = 100;
            comboStrokeStyle.SelectedIndexChanged += ComboStrokeStyle_SelectedIndexChanged;
            toolStrip1.Items.Add(new ToolStripControlHost(comboStrokeStyle));

            btnStrokeColor = new ToolStripButton("Цвет контура");
            btnStrokeColor.Click += StrokeColorMenuItem_Click;
            toolStrip1.Items.Add(btnStrokeColor);

            // Canvas Panel
            panelCanvas = new Panel();
            panelCanvas.Dock = DockStyle.Fill;
            panelCanvas.BackColor = Color.White;
            panelCanvas.Paint += PanelCanvas_Paint;
            panelCanvas.MouseDown += PanelCanvas_MouseDown;
            panelCanvas.MouseMove += PanelCanvas_MouseMove;
            panelCanvas.MouseUp += PanelCanvas_MouseUp;

            // ColorDialog
            colorDialog1 = new ColorDialog();

            // Add controls
            this.Controls.Add(panelCanvas);
            this.Controls.Add(toolStrip1);
            this.Controls.Add(menuStrip1);

            this.MainMenuStrip = menuStrip1;
        }

        private void InitializeStacks()
        {
            _undoStack = new StackMemory(20);
            _redoStack = new StackMemory(20);
            SaveState();
        }

        private void SaveState()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SaveToStream(ms, _figures);
                _undoStack.Push(ms);
                _redoStack.Clear();
            }
        }

        private void RestoreState(StackMemory stack)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stack.Pop(ms);
                var loadedFigures = LoadFromStream(ms);
                if (loadedFigures != null)
                {
                    _figures = loadedFigures.ToList();
                    _selectedFigure = null;
                    panelCanvas.Invalidate();
                }
            }
        }

        private void SaveToStream(Stream stream, List<Figure> listToSave = null)
        {
            var formatter = new BinaryFormatter();
            var list = (listToSave ?? _figures).ToList();
            formatter.Serialize(stream, list);
            stream.Position = 0;
        }

        private IEnumerable<Figure> LoadFromStream(Stream stream)
        {
            try
            {
                var formatter = new BinaryFormatter();
                stream.Position = 0;
                return (List<Figure>)formatter.Deserialize(stream);
            }
            catch (SerializationException)
            {
                return null;
            }
        }

        private void SetCurrentFigureType(Type type)
        {
            _currentFigureType = type;
            _selectedFigure = null;
            panelCanvas.Invalidate();
        }

        private void AddFigureAt(Point location)
        {
            Figure figure = null;
            int size = 80;

            if (_currentFigureType == typeof(RectangleFigure))
            {
                figure = new RectangleFigure(new Rectangle(location.X - size / 2, location.Y - size / 2, size, size), new Stroke());
            }
            else if (_currentFigureType == typeof(SquareFigure))
            {
                figure = new SquareFigure(new Rectangle(location.X - size / 2, location.Y - size / 2, size, size), new Stroke());
            }
            else if (_currentFigureType == typeof(LShapeFigure))
            {
                figure = new LShapeFigure(new Rectangle(location.X - size / 2, location.Y - size / 2, size, size), new Stroke());
            }
            else if (_currentFigureType == typeof(UShapeFigure))
            {
                figure = new UShapeFigure(new Rectangle(location.X - size / 2, location.Y - size / 2, size, size), new Stroke());
            }

            if (figure != null)
            {
                figure.Stroke.Width = (float)numericStrokeWidth.Value;
                figure.Stroke.DashStyle = GetDashStyleFromCombo();

                _figures.Add(figure);
                _selectedFigure = figure;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private DashStyle GetDashStyleFromCombo()
        {
            switch (comboStrokeStyle.SelectedIndex)
            {
                case 1: return DashStyle.Dash;
                case 2: return DashStyle.Dot;
                default: return DashStyle.Solid;
            }
        }

        private void UpdateSelectedFigureStroke()
        {
            if (_selectedFigure != null)
            {
                _selectedFigure.Stroke.Width = (float)numericStrokeWidth.Value;
                _selectedFigure.Stroke.DashStyle = GetDashStyleFromCombo();
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void PanelCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var figure in _figures)
            {
                figure.Draw(e.Graphics);
            }

            if (_selectedFigure != null)
            {
                SelectionMarkers.DrawMarkers(e.Graphics, _selectedFigure.Bounds);
            }
        }

        private void PanelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            panelCanvas.Focus();

            if (e.Button == MouseButtons.Left)
            {
                if (_selectedFigure != null)
                {
                    _resizeMarkerIndex = SelectionMarkers.HitTest(e.Location, _selectedFigure.Bounds);
                    if (_resizeMarkerIndex >= 0)
                    {
                        _isResizing = true;
                        _originalBounds = _selectedFigure.Bounds;
                        _dragStartPoint = e.Location;
                        return;
                    }
                }

                _selectedFigure = null;
                for (int i = _figures.Count - 1; i >= 0; i--)
                {
                    if (_figures[i].HitTest(e.Location))
                    {
                        _selectedFigure = _figures[i];
                        _isDragging = true;
                        _dragStartPoint = e.Location;
                        _originalLocation = _selectedFigure.Bounds.Location;
                        panelCanvas.Invalidate();
                        return;
                    }
                }

                AddFigureAt(e.Location);
                _isDragging = false;
            }
        }

        private void PanelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizing && _selectedFigure != null)
            {
                Rectangle newBounds = _originalBounds;
                int deltaX = e.Location.X - _dragStartPoint.X;
                int deltaY = e.Location.Y - _dragStartPoint.Y;

                switch (_resizeMarkerIndex)
                {
                    case 0:
                        newBounds = new Rectangle(_originalBounds.X + deltaX, _originalBounds.Y + deltaY,
                            _originalBounds.Width - deltaX, _originalBounds.Height - deltaY);
                        break;
                    case 1:
                        newBounds = new Rectangle(_originalBounds.X, _originalBounds.Y + deltaY,
                            _originalBounds.Width, _originalBounds.Height - deltaY);
                        break;
                    case 2:
                        newBounds = new Rectangle(_originalBounds.X, _originalBounds.Y + deltaY,
                            _originalBounds.Width + deltaX, _originalBounds.Height - deltaY);
                        break;
                    case 3:
                        newBounds = new Rectangle(_originalBounds.X, _originalBounds.Y,
                            _originalBounds.Width + deltaX, _originalBounds.Height);
                        break;
                    case 4:
                        newBounds = new Rectangle(_originalBounds.X, _originalBounds.Y,
                            _originalBounds.Width + deltaX, _originalBounds.Height + deltaY);
                        break;
                    case 5:
                        newBounds = new Rectangle(_originalBounds.X, _originalBounds.Y,
                            _originalBounds.Width, _originalBounds.Height + deltaY);
                        break;
                    case 6:
                        newBounds = new Rectangle(_originalBounds.X + deltaX, _originalBounds.Y,
                            _originalBounds.Width - deltaX, _originalBounds.Height + deltaY);
                        break;
                    case 7:
                        newBounds = new Rectangle(_originalBounds.X + deltaX, _originalBounds.Y,
                            _originalBounds.Width - deltaX, _originalBounds.Height);
                        break;
                }

                if (newBounds.Width > 20 && newBounds.Height > 20)
                {
                    _selectedFigure.Resize(newBounds.Width, newBounds.Height);
                    _selectedFigure.MoveTo(newBounds.Location);
                    panelCanvas.Invalidate();
                }
            }
            else if (_isDragging && _selectedFigure != null)
            {
                int dx = e.Location.X - _dragStartPoint.X;
                int dy = e.Location.Y - _dragStartPoint.Y;
                _selectedFigure.MoveTo(new Point(_originalLocation.X + dx, _originalLocation.Y + dy));
                panelCanvas.Invalidate();
            }
        }

        private void PanelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging || _isResizing)
            {
                _isDragging = false;
                _isResizing = false;
                _resizeMarkerIndex = -1;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_selectedFigure == null) return base.ProcessCmdKey(ref msg, keyData);

            int step = (ModifierKeys & Keys.Shift) != 0 ? 1 : 5;

            switch (keyData)
            {
                case Keys.Left:
                    _selectedFigure.Move(-step, 0);
                    break;
                case Keys.Right:
                    _selectedFigure.Move(step, 0);
                    break;
                case Keys.Up:
                    _selectedFigure.Move(0, -step);
                    break;
                case Keys.Down:
                    _selectedFigure.Move(0, step);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            SaveState();
            panelCanvas.Invalidate();
            return true;
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            _figures.Clear();
            _selectedFigure = null;
            SaveState();
            panelCanvas.Invalidate();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Vector files (*.vec)|*.vec|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open))
                {
                    var loaded = LoadFromStream(fs);
                    if (loaded != null)
                    {
                        _figures = loaded.ToList();
                        _selectedFigure = null;
                        panelCanvas.Invalidate();
                    }
                }
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Vector files (*.vec)|*.vec|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Create))
                {
                    SaveToStream(fs, _figures);
                }
            }
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            if (_undoStack.Count > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _undoStack.Pop(ms);
                    SaveToStream(ms, _figures);
                    _redoStack.Push(ms);

                    var restored = LoadFromStream(ms);
                    if (restored != null)
                    {
                        _figures = restored.ToList();
                        _selectedFigure = null;
                        panelCanvas.Invalidate();
                    }
                }
            }
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            if (_redoStack.Count > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _redoStack.Pop(ms);
                    var restored = LoadFromStream(ms);
                    if (restored != null)
                    {
                        _figures = restored.ToList();
                        _selectedFigure = null;
                        panelCanvas.Invalidate();
                    }
                }
            }
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _clipboardFigure = _selectedFigure.Clone();
            }
        }

        private void CutMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _clipboardFigure = _selectedFigure.Clone();
                _figures.Remove(_selectedFigure);
                _selectedFigure = null;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            if (_clipboardFigure != null)
            {
                Figure newFigure = _clipboardFigure.Clone();
                newFigure.Move(20, 20);
                _figures.Add(newFigure);
                _selectedFigure = newFigure;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _figures.Remove(_selectedFigure);
                _selectedFigure = null;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void FlipHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _selectedFigure.FlipHorizontally();
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void FlipVerticalMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _selectedFigure.FlipVertically();
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void RotateMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                _selectedFigure.Rotate90();
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void StrokeColorMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null && colorDialog1.ShowDialog() == DialogResult.OK)
            {
                _selectedFigure.Stroke.Color = colorDialog1.Color;
                SaveState();
                panelCanvas.Invalidate();
            }
        }

        private void NumericStrokeWidth_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedFigureStroke();
        }

        private void ComboStrokeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedFigureStroke();
        }
    }
}