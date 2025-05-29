import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";

const AllTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTaskComponents = taskStore.incompletedTasks.map(t => (
        <TaskListItem
            key={`inbox-${t.id}`}
            task={t}
        />
    ));

    const completedTaskComponents = taskStore.completedTasks.map(t => (
        <TaskListItem
            key={`inbox-${t.id}`}
            task={t}
        />
    ));

    return (
        <>
            <strong>Все задачи</strong>
            <Divider />
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default AllTaskList;
