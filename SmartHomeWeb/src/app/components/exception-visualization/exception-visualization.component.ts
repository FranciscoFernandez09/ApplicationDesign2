import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {NgClass, NgIf} from '@angular/common';

@Component({
  selector: 'app-exception-visualization',
  templateUrl: './exception-visualization.component.html',
  standalone: true,
  imports: [
    NgIf,
    NgClass
  ],
  styleUrls: ['./exception-visualization.component.css']
})
export class ExceptionVisualizationComponent implements OnChanges {
  @Input() text: string = '';
  animate: boolean = false;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['text']) {
      this.triggerAnimation();
    }
  }

  triggerAnimation() {
    this.animate = false;
    setTimeout(() => {
      this.animate = true;
    }, 10); // Slight delay to ensure the class is toggled
  }
}
