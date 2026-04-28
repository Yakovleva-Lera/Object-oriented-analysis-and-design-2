import tkinter as tk
import random
from abc import ABC, abstractmethod

# =============================================================================
# 1. STATE INTERFACE (абстрактный интерфейс состояния)
# =============================================================================
class CellState(ABC):
    @abstractmethod
    def update(self, context, grid, x, y):
        """Логика перехода в следующее состояние"""
        pass

    def set_context(self, context):
        """Связывание состояния с контекстом"""
        self.context = context

    @abstractmethod
    def get_color(self) -> str:
        pass

# =============================================================================
# 2. CONTEXT (класс-контекст)
# =============================================================================
class Cell:
    def __init__(self, state: CellState, cfg: dict = None):
        """Конструктор контекста с начальным состоянием: context(initialstate)"""
        self.state = state
        self.cfg = cfg
        self.state.set_context(self)

    def change_state(self, new_state: CellState):
        """Метод смены состояния: changestate()"""
        self.state = new_state
        self.state.set_context(self)

    def update(self, grid, x, y):
        """Делегирование поведения текущему состоянию"""
        self.state.update(self, grid, x, y)

    def get_color(self):
        """Делегирование получения цвета"""
        return self.state.get_color()

# =============================================================================
# 3. CONCRETE STATES (конкретные классы состояний)
# =============================================================================

class EmptyState(CellState):
    def get_color(self): 
        return '#d2b48c'

    def update(self, context, grid, x, y):
        if random.random() < context.cfg['prob_growth']:
            context.change_state(TreeState(0))

class TreeState(CellState):
    def __init__(self, age=0):
        self.age = age
        # self.context будет установлен через set_context() из CellState

    def get_color(self): 
        # Безопасная проверка: контекст может ещё не быть установлен
        if hasattr(self, 'context') and self.context and self.age > self.context.cfg.get('old_age_threshold', 20):
            return '#006400' 
        return '#32cd32'

    def update(self, context, grid, x, y):
        self.age += 1
        cfg = context.cfg
        will_burn = False
        
        rows, cols = len(grid), len(grid[0])
        wind = cfg['wind_dir']
        
        for dx in [-1, 0, 1]:
            for dy in [-1, 0, 1]:
                if dx == 0 and dy == 0: 
                    continue
                nx, ny = x + dx, y + dy
                if 0 <= nx < cols and 0 <= ny < rows:
                    neighbor = grid[ny][nx]
                    if isinstance(neighbor.state, BurningState):
                        if dx == -wind[0] and dy == -wind[1]:
                            will_burn = True
                        elif random.random() < 0.2:
                            will_burn = True
        
        prob = cfg['prob_fire']
        if self.age > cfg['old_age_threshold']: 
            prob *= 100
        if random.random() < prob: 
            will_burn = True
        
        if will_burn:
            context.change_state(BurningState())

class BurningState(CellState):
    def get_color(self): 
        return '#ff4500'

    def update(self, context, grid, x, y):
        context.change_state(EmptyState())

class WaterState(CellState):
    def get_color(self): 
        return '#4682b4'

    def update(self, context, grid, x, y):
        pass  # Вода не меняется

# =============================================================================
# 4. CLIENT / APP (использование паттерна)
# =============================================================================
class ForestFireApp:
    ROWS, COLS, CELL_SIZE = 100, 150, 6
    
    def __init__(self, root):
        self.root = root
        self.root.title("Forest Fire - State Pattern")
        
        self.cfg = {
            'prob_growth': 0.05,
            'prob_fire': 0.0005,
            'old_age_threshold': 40,
            'wind_dir': (1, 0)
        }
        
        self._init_ui()
        self._init_grid()
        self.running = False
        
    def _init_ui(self):
        self.canvas = tk.Canvas(
            self.root, width=self.COLS*self.CELL_SIZE, 
            height=self.ROWS*self.CELL_SIZE, bg='#000'
        )
        self.canvas.pack(pady=10)
        
        frame = tk.Frame(self.root)
        frame.pack()
        self.btn = tk.Button(frame, text="Start", command=self._toggle, width=12)
        self.btn.pack(side=tk.LEFT, padx=5)
        tk.Button(frame, text="Reset", command=self._reset, width=12).pack(side=tk.LEFT, padx=5)
        
        self.canvas.bind("<Button-1>", self._on_click)
        
    def _init_grid(self):
        random.seed(42)
        cx, cy = self.COLS//2, self.ROWS//2
        self.grid = []
        
        for y in range(self.ROWS):
            row = []
            for x in range(self.COLS):
                state = None
                if (cx-15 < x < cx+15) and (cy-10 < y < cy+10):
                    state = WaterState()
                elif random.random() < 0.55:
                    state = TreeState(random.randint(0, 25))
                else:
                    state = EmptyState()
                # Клиент создаёт Контекст с начальным состоянием
                row.append(Cell(state, self.cfg))
            self.grid.append(row)
        self._draw()
        
    def _draw(self):
        self.canvas.delete("all")
        for y in range(self.ROWS):
            for x in range(self.COLS):
                self.canvas.create_rectangle(
                    x*self.CELL_SIZE, y*self.CELL_SIZE,
                    (x+1)*self.CELL_SIZE, (y+1)*self.CELL_SIZE,
                    fill=self.grid[y][x].get_color(), outline=""
                )
                
    def step(self):
        # Double Buffering: создаём новую сетку на основе старой
        next_grid = []
        for y in range(self.ROWS):
            new_row = []
            for x in range(self.COLS):
                old_cell = self.grid[y][x]
                # Создаём новый Контекст, копируя состояние
                new_cell = Cell(old_cell.state, self.cfg)
                # Обновляем новый контекст на основе старой сетки
                new_cell.update(self.grid, x, y)
                new_row.append(new_cell)
            next_grid.append(new_row)
        self.grid = next_grid
        self._draw()
        
    def _loop(self):
        if self.running:
            self.step()
            self.root.after(50, self._loop)
            
    def _toggle(self):
        self.running = not self.running
        self.btn.config(text="Stop" if self.running else "Start")
        if self.running: 
            self._loop()
        
    def _reset(self):
        self.running = False
        self.btn.config(text="Start")
        self._init_grid()
        
    def _on_click(self, event):
        x, y = event.x // self.CELL_SIZE, event.y // self.CELL_SIZE
        if 0 <= x < self.COLS and 0 <= y < self.ROWS:
            if not isinstance(self.grid[y][x].state, WaterState):
                self.grid[y][x].change_state(BurningState())
                self._draw()
                if not self.running: 
                    self._toggle()

def main():
    root = tk.Tk()
    root.resizable(False, False)
    ForestFireApp(root)
    root.mainloop()

if __name__ == "__main__":
    main()