import {Component, Input} from '@angular/core';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-success-visualization',
  templateUrl: './success-visualization.component.html',
  standalone: true,
  imports: [NgIf],
  styleUrls: ['./success-visualization.component.css']
})
export class SuccessVisualizationComponent {
  @Input() text: string = '';
}
