import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/RootStore";
import TaskListView from "./TaskListView";

const AllTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.incompletedTasks;

    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListView
            key={`inbox-${t.id}`}
            task={t}
        />
    ));

    const completedTaskComponents = taskStore.completedTasks.map(t => (
        <TaskListView
            key={`inbox-${t.id}`}
            task={t}
        />
    ));

    return (
        <>
            <strong>
                Все задачи (не выполнено задач: {incompletedTasks.length})
            </strong>
            {incompletedTaskComponents}
            {completedTaskComponents}
        </>
    );
});

export default AllTaskList;
