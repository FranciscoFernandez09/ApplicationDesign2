import {Component, EventEmitter, Input, Output} from '@angular/core';
import CreateDeviceModel from '../../../backend/services/company-owner/models/CreateDeviceModel';
import {FormComponent} from '../../components/form/form.component';
import DeviceImageModel from '../../../backend/services/company-owner/models/DeviceImageModel';

@Component({
  selector: 'app-create-device',
  standalone: true,
  imports: [FormComponent],
  templateUrl: './create-device.component.html',
  styleUrl: './create-device.component.css',
})
export class CreateDeviceComponent {
  @Output() deviceSubmit = new EventEmitter<any>();
  @Input() formTitle?: string = '';

  formElements = [
    {name: 'name', type: 'text', label: 'Name'},
    {name: 'model', type: 'text', label: 'Model'},
    {name: 'description', type: 'text', label: 'Description'},
    {name: 'mainImage', type: 'text', label: 'Main Image'},
    {name: 'images', type: 'text', label: 'Images', multiple: true},
  ];

  constructor() {
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    let parsedImages = this.parseImage(formData);
    const newDevice: CreateDeviceModel = {
      name: formData.name,
      model: formData.model,
      description: formData.description,
      images: parsedImages,
    };
    this.deviceSubmit.emit(newDevice);
  }

  parseImage(data: any): Array<DeviceImageModel> {
    let parseImages: Array<DeviceImageModel> = [];

    if (data.mainImage) {
      parseImages.push({
        isMain: true,
        imageUrl: data.mainImage,
      });
    }

    if (typeof data.images === 'string' && data.images.trim() !== '') {
      const imageArray = data.images
        .split(',')
        .map((image: string) => image.trim());
      parseImages = parseImages.concat(
        imageArray.map((image: string) => {
          return {
            isMain: false,
            imageUrl: image,
          } as DeviceImageModel;
        })
      );
    }

    return parseImages;
  }
}
