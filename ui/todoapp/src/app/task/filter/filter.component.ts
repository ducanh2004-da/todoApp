import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TaskStatus } from '../enums/task-status.enum';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html'
})
export class FilterComponent {
  @Output() statusChange = new EventEmitter<TaskStatus>();

  // Expose enum to template
  public TaskStatus = TaskStatus;
}
