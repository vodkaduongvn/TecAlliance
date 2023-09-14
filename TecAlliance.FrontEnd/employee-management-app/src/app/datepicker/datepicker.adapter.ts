import {Component, Injectable} from '@angular/core';
import {NgbDateAdapter, NgbDateStruct, NgbDateNativeAdapter,NgbDateParserFormatter} from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class CustomDateAdapter {
  fromModel(value: string): NgbDateStruct
  {
     let parts=value.split('-');
     return {year:+parts[0],month:+parts[1],day:+parts[2]} as NgbDateStruct
  }

  toModel(date: NgbDateStruct): string // from internal model -> your mode
  {
    return date?date.year+"-"+('0'+date.month).slice(-2)+"-"+('0'+date.day).slice(-2):'';
  }

}
@Injectable()
export class CustomDateParserFormatter {
  parse(value: string): NgbDateStruct
  {
     let parts=value.split('/');
     return {year:+parts[0],month:+parts[1],day:+parts[2]} as NgbDateStruct

  }
  format(date: NgbDateStruct): string
  {
    return date?date.year+"-"+('0'+date.month).slice(-2)+"-"+('0'+date.day).slice(-2):'';
  }
}
