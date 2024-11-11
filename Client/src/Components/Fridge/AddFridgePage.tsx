import React, { useContext, useState } from 'react';
import './FridgePage.scss';
import { Context } from '../..';
import FridgeService from '../../services/FridgeService';
import { useNavigate } from 'react-router-dom';
import { IFridge } from '../../models/Fridge';

export const AddFridgePage: React.FC = () => {
  const [serialNumber, setSerialNumber] = useState<string>('');
  const [boxNumber, setBoxNumber] = useState<number>(0);

  const [createNew, setCreateNew] = useState<boolean>(false); 
  const [error, setError] = useState<string>('');
  const { store } = useContext(Context);
  const history = useNavigate();
    const [fridge,setFridge] = useState({} as IFridge); 


  const handleInputChangeSerial = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSerialNumber(e.target.value);
  };

  const handleInputChangeBoxNumber = (e: React.ChangeEvent<HTMLInputElement>) => {
    setBoxNumber(Number(e.target.value));
  }

  const handleCreateNewChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCreateNew(e.target.checked);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      let response;
      if (createNew) {
        response = await FridgeService.AddFridge(fridge);
      } else {
        response = await FridgeService.addFridgeToUser(store.user.Id,boxNumber, serialNumber);
      }

      if (response.status === 200) {
        setError('');
        history('/');
      } else {
        setError('Error');
      }
    } catch (e: any) {
      console.error(e);
      setError('Error')
    }
  };

  const convertToUTC = (date: string) => {
    const localDate = new Date(date);
    return new Date(Date.UTC(
      localDate.getFullYear(),
      localDate.getMonth(),
      localDate.getDate(),
      localDate.getHours(),
      localDate.getMinutes(),
      localDate.getSeconds()
    )).toISOString(); // Преобразуем дату в строку UTC-формата
  };
  
  return (
    <div className="fridge-form-container">
      <h1 className="title">Add Fridge</h1>
      <form onSubmit={handleSubmit} className="fridge-form">
        <label className="form-label flex-label">
          <input
            type="checkbox"
            checked={createNew}
            onChange={handleCreateNewChange}
          />
          Create New Fridge
        </label>

        {createNew ? (
            <>
            <label htmlFor="name" className="form-label">Name</label>
            <input
              type="text"
              id="name"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={fridge.Name}
              onChange={(e)=>setFridge({...fridge,Name:e.target.value})}
              placeholder="Enter Name"
            />
            <label htmlFor="model" className="form-label">Model</label>
            <input
              type="text"
              id="model"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={fridge.Model}
              onChange={(e)=>setFridge({...fridge,Model:e.target.value})}
              placeholder="Enter Model"
            />
            <label htmlFor="boughtDate" className="form-label">Bought Date</label>
            <input
              type="date"
              id="boughtDate"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={fridge.BoughtDate ? fridge.BoughtDate.substring(0, 10) : ''}
              onChange={(e)=>setFridge({...fridge,BoughtDate:convertToUTC(e.target.value)})}
            />
            <label htmlFor="boxNumber" className="form-label">Box Number</label>
            <input
              type="number"
              id="boxNumber"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={fridge.BoxNumber}
              onChange={(e)=>setFridge({...fridge,BoxNumber:Number(e.target.value)})}
              placeholder="Enter box number"
            />
            <label htmlFor="Serial" className="form-label">Serial</label>
            <input
              type="text"
              id="Serial"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={fridge.Serial}
              onChange={(e)=>setFridge({...fridge,Serial:e.target.value})}
              placeholder="Enter Serial"
            />
            </>
        ): (
          <>
            <label htmlFor="serialNumber" className="form-label">Serial</label>
            <input
              type="text"
              id="serialNumber"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={serialNumber}
              onChange={handleInputChangeSerial}
              placeholder="Enter Serial Number"
            />
            <label htmlFor="boxNumber" className="form-label">Box Number</label>
            <input
              type="number"
              id="boxNumber"
              className={`form-input ${error ? 'input-error' : ''}`}
              value={boxNumber}
              onChange={handleInputChangeBoxNumber}
              placeholder="Enter Serial Number"
            />

          </>
        )}

        {error && <p className="error-message">{error}</p>}
        <button type="submit" className="submit-button">
          {createNew ? 'Create' : 'Add'}
        </button>
      </form>
    </div>
  );
};