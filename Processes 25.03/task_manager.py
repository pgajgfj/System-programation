import tkinter as tk
from tkinter import ttk, messagebox, filedialog
import psutil
import os
import datetime

class TaskManagerApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Task Manager")
        self.root.geometry("800x500")

        # Таблиця для відображення процесів
        self.tree = ttk.Treeview(root, columns=("PID", "Name", "Start Time", "Status", "Priority"), show="headings")
        self.tree.heading("PID", text="PID")
        self.tree.heading("Name", text="Process Name")
        self.tree.heading("Start Time", text="Start Time")
        self.tree.heading("Status", text="Status")
        self.tree.heading("Priority", text="Priority")

        self.tree.column("PID", width=50)
        self.tree.column("Name", width=200)
        self.tree.column("Start Time", width=150)
        self.tree.column("Status", width=100)
        self.tree.column("Priority", width=80)

        self.tree.pack(expand=True, fill=tk.BOTH)

        # Кнопки керування
        self.btn_frame = tk.Frame(root)
        self.btn_frame.pack(fill=tk.X)

        self.refresh_btn = tk.Button(self.btn_frame, text="Refresh", command=self.update_processes)
        self.refresh_btn.pack(side=tk.LEFT, padx=5, pady=5)

        self.kill_btn = tk.Button(self.btn_frame, text="Kill Process", command=self.kill_process)
        self.kill_btn.pack(side=tk.LEFT, padx=5, pady=5)

        self.details_btn = tk.Button(self.btn_frame, text="Details", command=self.show_details)
        self.details_btn.pack(side=tk.LEFT, padx=5, pady=5)

        self.run_btn = tk.Button(self.btn_frame, text="Run Process", command=self.run_process)
        self.run_btn.pack(side=tk.LEFT, padx=5, pady=5)

        # Випадаючий список для вибору інтервалу оновлення
        self.interval_var = tk.IntVar(value=2)
        self.interval_menu = ttk.Combobox(self.btn_frame, textvariable=self.interval_var, values=[1, 2, 5])
        self.interval_menu.pack(side=tk.RIGHT, padx=5)
        self.interval_menu.bind("<<ComboboxSelected>>", lambda e: self.set_update_interval())

        self.auto_update()
    
    def get_processes(self):
        """Отримання списку процесів"""
        processes = []
        for proc in psutil.process_iter(attrs=['pid', 'name', 'create_time', 'status', 'nice']):
            try:
                start_time = datetime.datetime.fromtimestamp(proc.info['create_time']).strftime('%Y-%m-%d %H:%M:%S')
                processes.append((proc.info['pid'], proc.info['name'], start_time, proc.info['status'], proc.info['nice']))
            except (psutil.NoSuchProcess, psutil.AccessDenied, psutil.ZombieProcess):
                continue
        return processes

    def update_processes(self):
        """Оновлення таблиці процесів"""
        for row in self.tree.get_children():
            self.tree.delete(row)

        for proc in self.get_processes():
            self.tree.insert("", tk.END, values=proc)

    def kill_process(self):
        """Завершення вибраного процесу"""
        selected_item = self.tree.selection()
        if not selected_item:
            messagebox.showwarning("Warning", "Select a process to kill")
            return
        
        pid = self.tree.item(selected_item, "values")[0]
        try:
            proc = psutil.Process(int(pid))
            proc.terminate()
            messagebox.showinfo("Success", f"Process {pid} terminated")
            self.update_processes()
        except Exception as e:
            messagebox.showerror("Error", str(e))

    def show_details(self):
        """Перегляд деталей процесу"""
        selected_item = self.tree.selection()
        if not selected_item:
            messagebox.showwarning("Warning", "Select a process to view details")
            return
        
        pid = self.tree.item(selected_item, "values")[0]
        try:
            proc = psutil.Process(int(pid))
            details = f"PID: {proc.pid}\nName: {proc.name()}\nStatus: {proc.status()}\n" \
                      f"Start Time: {datetime.datetime.fromtimestamp(proc.create_time()).strftime('%Y-%m-%d %H:%M:%S')}\n" \
                      f"Priority: {proc.nice()}\nMemory Usage: {proc.memory_info().rss / (1024 * 1024):.2f} MB"
            
            messagebox.showinfo("Process Details", details)
        except Exception as e:
            messagebox.showerror("Error", str(e))

    def run_process(self):
        """Запуск нового процесу"""
        file_path = filedialog.askopenfilename(filetypes=[("Executable Files", "*.exe")])
        if file_path:
            try:
                os.startfile(file_path)
                messagebox.showinfo("Success", f"Process {file_path} started")
                self.update_processes()
            except Exception as e:
                messagebox.showerror("Error", str(e))

    def auto_update(self):
        """Автоматичне оновлення списку процесів"""
        self.update_processes()
        self.root.after(self.interval_var.get() * 1000, self.auto_update)

    def set_update_interval(self):
        """Змінити інтервал оновлення"""
        self.root.after_cancel(self.auto_update)
        self.auto_update()

# Запуск програми
if __name__ == "__main__":
    root = tk.Tk()
    app = TaskManagerApp(root)
    root.mainloop()
