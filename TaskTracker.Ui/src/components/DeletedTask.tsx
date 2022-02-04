import React from 'react';
import IDeletedTask from '../interfaces/IDeletedTask';

class DeletedTask extends React.Component<IDeletedTaskProps> {
    render() {
        const task = this.props.task;
        const deletionDate = task.deletionDate
            ? <span>Deletion date: {task.deletionDate}</span>
            : null;

        return (
            <div>
                <span style={{ color: 'grey' }}>
                    ID: {task.id}<br />
                    Title: {task.title}<br />
                    Description: {task.description}<br />
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
