import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormComponent} from '../../components/form/form.component';
import {HomeOwnerService} from '../../../backend/services/home-management/home-owner/home-owner.service';
import {ServerResponseVisualization} from '../server-response-visualization/server-response-visualization';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-load-homes',
  standalone: true,
  imports: [FormComponent, ServerResponseVisualization, NgIf],
  templateUrl: './load-homes.component.html',
  styleUrl: './load-homes.component.css',
})
export class LoadHomesComponent implements OnInit {
  @Output() onHomeSubmit = new EventEmitter<any>();

  homeFormElements = [
    {name: 'homeId', type: 'select-with-values', label: 'Home', values: [], options: []},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService) {
  }

  ngOnInit() {
    this.loadHomes();
  }

  onFormSubmit(formValues: any) {
    this.onHomeSubmit.emit(formValues);
  }

  loadHomes() {
    this.homeOwnerService.getMineHomes().subscribe((response: any) => {
        this.homeFormElements[0].values = response.map((home: any) => home.homeId);
        this.homeFormElements[0].options = response.map((home: any) => home.name);
        this.message = '';
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }

}
