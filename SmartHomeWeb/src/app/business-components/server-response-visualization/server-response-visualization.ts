import {Component, Input} from '@angular/core';
import {SuccessVisualizationComponent} from '../../components/success-visualization/success-visualization.component';
import {NgIf} from '@angular/common';
import {
  ExceptionVisualizationComponent
} from '../../components/exception-visualization/exception-visualization.component';

@Component({
  selector: 'app-server-response-visualization',
  standalone: true,
  imports: [
    SuccessVisualizationComponent,
    NgIf,
    ExceptionVisualizationComponent
  ],
  templateUrl: './server-response-visualization.html',
  styleUrl: './server-response-visualization.css',
})
export class ServerResponseVisualization {
  @Input() message: any = '';
  @Input() isSuccess?: boolean = false;

  constructor() {
  }
}
