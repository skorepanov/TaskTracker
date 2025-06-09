import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const AllTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getIncompletedTasks(true);
    const completedTasks = taskStore.getCompletedTasks(true);

    return (
        <>
            <div style={{ paddingLeft: 10, paddingTop: 10 }}>
                <strong>Все задачи</strong>
            </div>
            <TaskCreationPanel />
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default AllTaskList;
