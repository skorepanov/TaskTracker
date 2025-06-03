import React from "react";
import { observer } from "mobx-react-lite";
import { Button } from "antd";
import TaskCreationalModal from "./TaskCreationalModal";

interface ITaskCreationalModalProps {
    dueDateTime?: Date;
    folderId?: number;
}

const TaskCreationModalButton: React.FC<ITaskCreationalModalProps> = observer(
    props => {
        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleCreateTaskButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideTaskCreationModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <Button onClick={handleCreateTaskButtonClick}>
                    Новая задача
                </Button>
                <TaskCreationalModal
                    dueDateTime={props.dueDateTime}
                    folderId={props.folderId}
                    isModalOpen={isModalOpen}
                    hideModal={hideTaskCreationModal}
                />
            </>
        );
    }
);

export default TaskCreationModalButton;
