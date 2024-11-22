import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {NgForOf, NgIf} from '@angular/common';
import {FormComponent} from '../form/form.component';

@Component({
  selector: 'app-table-navigator',
  standalone: true,
  imports: [FormsModule, NgForOf, FormComponent, NgIf],
  templateUrl: './table-navigator.component.html',
  styleUrl: './table-navigator.component.css'
})
export class TableNavigatorComponent {
  @Input() formElements: {
    name: string;
    type: string;
    label: string;
    options?: string[];
  }[] = [];
  @Input() buttonText?: string = 'Submit';

  @Output() pageChange: EventEmitter<{ offset: number, limit: number }> = new EventEmitter<{
    offset: number,
    limit: number
  }>();


  page: number = 1;
  offset: number = 0;
  limit: number = 10;
  tableNavigatorData: {
    offset: number,
    limit: number,
    formData: any
  } = {
    offset: this.offset,
    limit: this.limit,
    formData: {}
  };


  nextPage() {
    this.offset += Number(this.limit);
    this.page++;
    this.emitChange();
  }

  onLimitChange(limit
                  :
                  number
  ) {
    this.limit = Number(limit);
    this.offset = 0;
    this.page = 1;
    this.emitChange();
  }

  prevPage() {
    if (this.offset > 0) {
      this.page--;
      this.offset -= Number(this.limit);
      this.emitChange();
    }
  }

  emitChange() {
    this.tableNavigatorData.offset = this.offset;
    this.tableNavigatorData.limit = this.limit;
    this.pageChange.emit(this.tableNavigatorData);
  }

  onFormSubmit(formData: any) {
    this.tableNavigatorData.formData = formData;
    this.emitChange();
  }
}
