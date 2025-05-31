import React from "react";
import { observer } from "mobx-react-lite";
import { Button } from "antd";
import TaskCreationalModal from "./TaskCreationalModal";

const TaskCreationModalButton: React.FC = observer(() => {
    const [isModalOpen, setIsModalOpen] = React.useState(false);

    const handleCreateTaskButtonClick = () => {
        setIsModalOpen(true);
    };

    const hideTaskCreationModal = () => {
        setIsModalOpen(false);
    };

    return (
        <>
            <Button onClick={handleCreateTaskButtonClick}>Новая задача</Button>
            <TaskCreationalModal
                isModalOpen={isModalOpen}
                hideModal={hideTaskCreationModal}
            />
        </>
    );
});

export default TaskCreationModalButton;
