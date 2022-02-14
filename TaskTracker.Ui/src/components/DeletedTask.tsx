import React from 'react';
import IDeletedTask from '../interfaces/IDeletedTask';

class DeletedTask extends React.Component<IDeletedTaskProps> {
    render() {
        const task = this.props.task;

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const deletionDate = task.deletionDate
            ? <span>Дата удаления: {task.deletionDate}</span>
            : null;

        return (
            <div style={{ color: 'grey', marginBottom: 10 }}>
                [{task.id}] {task.title}<br />
                {description}
                {deletionDate}
            </div>
        );
    }
}

interface IDeletedTaskProps {
    task: IDeletedTask;
}

export default DeletedTask;
