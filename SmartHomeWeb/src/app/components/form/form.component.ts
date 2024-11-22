import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {DatePipe, NgForOf, NgIf, NgStyle} from '@angular/common';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, NgStyle, DatePipe],
  styleUrls: ['./form.component.css'],
})
export class FormComponent implements OnInit {
  @Input() formElements: {
    name: string;
    type: string;
    label: string;
    options?: string[];
    values?: string[];
    required?: boolean;
  }[] = [];
  @Input() formTitle?: string = '';
  @Input() buttonText?: string = 'Submit';
  @Input() buttonColor?: string = '#044389';
  @Output() formSubmit = new EventEmitter<any>();

  formData: any = {};

  ngOnInit() {
    this.formElements.forEach(element => {
      if (element.type === 'number' || element.type === 'select-with-values') {
        this.formData[element.name] = null;
      } else {
        this.formData[element.name] = '';
      }
    });
  }

  onSubmit() {
    this.formSubmit.emit(this.formData);
  }
}
