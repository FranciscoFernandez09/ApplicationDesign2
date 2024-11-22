import {Component} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';
import CreateCameraModel from '../../../../../backend/services/company-owner/models/CreateCameraModel';
import DeviceImageModel from '../../../../../backend/services/company-owner/models/DeviceImageModel';

@Component({
  selector: 'app-create-camera',
  templateUrl: './create-camera.component.html',
  styleUrl: './create-camera.component.css',
  providers: [CompanyOwnerService],
})
export class CreateCameraComponent {
  formElements = [
    {name: 'name', type: 'text', label: 'Name', required: true},
    {name: 'model', type: 'text', label: 'Model', required: true},
    {name: 'description', type: 'text', label: 'Description', required: true},
    {
      name: 'hasExternalUse',
      type: 'radio',
      label: 'Has External Use',
      options: ['Yes', 'No'],
    },
    {
      name: 'hasInternalUse',
      type: 'radio',
      label: 'Has Internal Use',
      options: ['Yes', 'No'],
    },
    {
      name: 'hasMotionDetection',
      type: 'radio',
      label: 'Has Motion detection',
      options: ['Yes', 'No'],
    },
    {
      name: 'hasPersonDetection',
      type: 'radio',
      label: 'HasPerson Detection',
      options: ['Yes', 'No'],
    },
    {name: 'mainImage', type: 'text', label: 'Main Image', required: true},
    {name: 'images', type: 'text', label: 'Images', multiple: true},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _companyOwnerService: CompanyOwnerService) {
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    let parsedImages = this.parseImage(formData);
    let hasInternalUse = formData.hasInternalUse === 'Yes';
    let hasMotionDetection = formData.hasMotionDetection === 'Yes';
    const newCamera: CreateCameraModel = {
      name: formData.name,
      model: formData.model,
      description: formData.description,
      hasExternalUse: formData.hasExternalUse === 'Yes',
      hasInternalUse: formData.hasInternalUse === 'Yes',
      motionDetection: hasMotionDetection,
      personDetection: formData.hasPersonDetection === 'Yes',
      images: parsedImages,
    };
    this._companyOwnerService.createCamera(newCamera).subscribe((response: any) => {
      console.log('Camera created:', response);
      this.message = response;
      this.isSuccess = true;
    }, (error) => {
      console.error('Error:', error);
      this.message = error.details;
      this.isSuccess = false;
    });
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
