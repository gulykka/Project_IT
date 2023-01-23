﻿namespace Domain.Models; 

public interface IRepository<T> where T: class {
    public T Create(T item);
    public T Get(int id);
    public bool Delete(int id);
    public T Update(int id);
}