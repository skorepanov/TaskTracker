import React from 'react';
import IDeletedTask from '../interfaces/IDeletedTask';

class DeletedTask extends React.Component<IDeletedTaskProps> {
    render() {
        const task = this.props.task;
        const deletionDate = task.deletionDate
            ? <span>Дата удаления: {task.deletionDate}</span>
            : null;

        return (
            <div>
                <span style={{ color: 'grey' }}>
                    ID: {task.id}<br />
                    Заголовок: {task.title}<br />
                    Описание: {task.description}<br />
                    {deletionDate}
                </span>
            </div>
        );
    }
}

interface IDeletedTaskProps {
    task: IDeletedTask;
}

export default DeletedTask;
