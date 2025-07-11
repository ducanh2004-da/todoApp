import { ElementRef, OnInit, OnDestroy, Component, ViewChild } from '@angular/core';
import { TaskService } from './services/task.service';
import { TaskStatus } from './enums/task-status.enum';
import { Tag } from './models/tag.model';
import { Task } from './models/task.model';
import { TaskFormComponent } from './task-form/taskForm.component';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  @ViewChild('taskModal') taskModalRef!: ElementRef;
  @ViewChild(TaskFormComponent) formComp!: TaskFormComponent;

  public TaskStatus = TaskStatus;

  tasks: Task[] = [];
  tags: Tag[] = [];
  taskDetails: Task | null = null;

  modalTitle = '';
  selectedStatus: TaskStatus = TaskStatus.All;

  editData: any = null;

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.taskService.loadData(this.selectedStatus).subscribe({
      next: ([tasks, tags]) => {
        this.tasks = tasks;
        this.tags = tags;
      },
      error: () => alert('Failed to load data')
    });
  }

  onStatusChange(status: TaskStatus) {
    this.selectedStatus = status;
    this.loadData();
  }

  addClick() {
    this.modalTitle = 'Add Task';
    this.editData = null;
  }

  editClick(task: Task) {
    this.modalTitle = 'Edit Task';
    this.editData = {
      TaskId: task.TaskId,
      TaskTitle: task.TaskTitle,
      Description: task.Description,
      Priority: task.Priority,
      StartDay: task.StartDay,
      EndDay: task.EndDay,
      IsDone: task.IsDone,
      TagId: task.TaskTags[0]?.TagId
    };
  }

  viewDetails(id: number) {
    this.taskService.getTaskById(id).subscribe({
      next: data => this.taskDetails = data,
      error: () => alert('Failed to load details')
    });
  }

  deleteClick(id: number) {
    if (!confirm('Bạn có chắc chắn muốn xóa?')) return;
    this.taskService.deleteTask(id).subscribe({
      next: () => this.loadData(),
      error: () => alert('Failed to delete')
    });
  }

  doneTask(id: number) {
    if (!confirm('Đánh dấu hoàn thành?')) return;
    this.taskService.completeTask(id).subscribe({
      next: () => this.loadData(),
      error: () => alert('Failed to mark done')
    });
  }

  exportCsv() {
    this.taskService.exportCsv(this.selectedStatus).subscribe({
      next: blob => {
        const fileName = `tasks_${new Date().toISOString().slice(0,19).replace(/:/g,'')}.csv`;
        saveAs(blob, fileName);
      },
      error: () => alert('Failed to export CSV')
    });
  }

  onFormSubmit(formValue: any) {
    if (this.modalTitle === 'Add Task') {
      this.taskService.createTask(formValue).subscribe({
        next: () => this.loadData(),
        error: () => alert('Failed to create task')
      });
    } else {
      this.taskService.updateTask(formValue).subscribe({
        next: () => this.loadData(),
        error: () => alert('Failed to update task')
      });
    }
    this.taskModalRef.nativeElement.querySelector('.btn-close').click();
  }
}
